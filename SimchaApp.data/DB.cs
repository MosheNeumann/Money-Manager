using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaApp.data
{
    public class DB
    {
        public DB(string connectionString)
        {
            _connectionString = connectionString;
        }
        private string _connectionString;

        public List<Contributor> GetAllContributors()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"select c.* from Contributors C";

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<Contributor> AllContributors = new List<Contributor>();
                while (reader.Read())
                {

                    Contributor C = new Contributor();
                    C.Id = (int)reader["Id"];
                    C.Name = (string)reader["Name"];
                    C.Cell = (string)reader["cell"];
                    C.AlwaysInclude = (bool)reader["AlwaysIncude"];

                    C.Balance = GetBalance(C.Id);
                    AllContributors.Add(C);
                }
                return AllContributors;
            }
                
        }

        private int GetBalance(int id)
        {
           using (SqlConnection connection = new SqlConnection(_connectionString))

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "select (select isnull(sum(amount),0) from Deposits where contributorsId= @id) - (select isnull(sum(amount),0) from Contributions where contributersId=@id) as totalbalance";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                int x = 0;
                while (reader.Read())
                {

                    x = (int)(decimal)reader["totalbalance"];

                }

                return x;
            }
           
        }
        public List<Transaction> ShowHistoryByContributorID(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT  D.Amount,d.Date,d.name FROM Deposits D WHERE contributorsId=@id
                                    UNION ALL
                                   SELECT C.amount,c.date,s.Name  FROM Contributions C
                                     JOIN Simchas S
                                    ON S.Id=C.contributersId
                                     WHERE s.id=@id
                                     ORDER BY DATE";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                List<Transaction> AllTransactions = new List<Transaction>();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    AllTransactions.Add(new Transaction
                    {
                        Action = (string)reader["name"],
                        Date = (DateTime)reader["Date"],
                        Amount = (int)(decimal)reader["Amount"]
                    });
                }
                return AllTransactions;
            }
               
        }

        public Contributor GetContributorById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"select * from Contributors";
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Contributor C = new Contributor();
                while (reader.Read())
                {
                    C.Id = (int)reader["Id"];
                    C.Name = (string)reader["Name"];
                    C.Cell = (string)reader["cell"];
                    C.AlwaysInclude = (bool)reader["AlwaysIncude"];
                    C.Balance = GetBalance(id);
                }
                return C;
            }
               

        }

        public void AddContributor(Contributor C)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))

            using (SqlCommand command = connection.CreateCommand())
            {

                command.CommandText = "INSERT INTO Contributors VALUES( @NAME,@CELL,@ALWAYSINCLUDE) SELECT @@IDENTITY";
                command.Parameters.AddWithValue("@NAME", C.Name);
                command.Parameters.AddWithValue("@CELL", C.Cell);
                command.Parameters.AddWithValue("@ALWAYSINCLUDE", C.AlwaysInclude);
                connection.Open();
                C.Id = (int)(Decimal)command.ExecuteScalar();

            }
        }
        public void AddContribution(Contribution contribution)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Contributions VALUES( @contributersId,@SimchaId,@Amount)";
                command.Parameters.AddWithValue("@contributersId", contribution.ContributersId);
                command.Parameters.AddWithValue("@SimchaId", contribution.SimchaId);
                command.Parameters.AddWithValue("@Amount", contribution.Amount);
                connection.Open();
                command.ExecuteNonQuery();
            }
               
        }
        public void AddDeposit(Deposit deposit)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Deposits VALUES(@AMOUNT,@DATE,@CONTRIBUTORSID, @NAME) SELECT @@IDENTITY";
                command.Parameters.AddWithValue("@AMOUNT", deposit.Amount);
                command.Parameters.AddWithValue("@DATE", deposit.Date);
                command.Parameters.AddWithValue("@CONTRIBUTORSID", deposit.ContributorsId);
                command.Parameters.AddWithValue("@NAME", "Deposit");

                connection.Open();
                deposit.Id = (int)(decimal)command.ExecuteScalar();
            }
               
        }
        public void AddNewSimcha(Simcha simcha)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Simchas VALUES(@Name,@Date) SELECT @@IDENTITY";
                command.Parameters.AddWithValue("@Name", simcha.Name);
                command.Parameters.AddWithValue("@DATE", simcha.Date);
                connection.Open();
                simcha.Id = (int)(decimal)command.ExecuteScalar();
            }
                
        }
        public int GetTotalCurrentlyInFund()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "select (select sum(Amount) from Deposits) -  (select sum(amount) from Contributions) as TotalInFund";

                connection.Open();
                int TotalInFund = (int)(decimal)command.ExecuteScalar();
                return TotalInFund;
            }
              

           
        }
        public List<Simcha> GetAllSimchas()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT S.*,COUNT(C.contributersId) AS ContributorCount,SUM(C.AMOUNT) as Total FROM Simchas S
                                 left   JOIN Contributions C
                                     ON C.SimchaId=S.ID
                                    GROUP BY S.Id,S.Date,S.Name ";
                connection.Open();
                List<Simcha> AllSimchas = new List<Simcha>();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Simcha S = new Simcha();
                    S.Name = (string)reader["Name"];
                    S.Id = (int)reader["Id"];
                    S.Date = (DateTime)reader["Date"];
                    S.ContributorCount = (int)reader["ContributorCount"];
                    if (reader["Total"] != DBNull.Value)
                    {
                        S.Total = (int)(decimal)reader["Total"];
                    }
                    AllSimchas.Add(S);
                }
                return AllSimchas;
            }
               
        }

        public int TotalContributors()
        {

            SqlConnection connection = new SqlConnection(_connectionString);

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(C.Id)  FROM Contributors C";
            connection.Open();

            int total = (int)(decimal)command.ExecuteScalar();

            return total;
        }
        public List<string> GetContributorsForSimchaList(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"select c.name from Contributors C
                                   join Contributions CO
                                   on c.Id=co.contributersId
                                   group by c.Name,co.SimchaId  
                                 having co.SimchaId=@id ";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            List<string> names = new List<string>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string name = (string)reader["name"];
                names.Add(name);

            }
            return names;
        }

        public Simcha GetSimchaByID(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "select * from Simchas where id=@ID";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            Simcha S = new Simcha();
            while (reader.Read())
            {
                S.Id = (int)reader["id"];
                S.Name = (string)reader["name"];
                S.Date = (DateTime)reader["Date"];
            }
            return S;
        }


        public void PostcontributionsForSimcha(List<Contribution> List)
        {
            DeletecontributionsFromSimchaBySimchaId(List[0].SimchaId);



            foreach (Contribution C in List)
            {
                AddContributionsToDB(C);

            }


        }
        private void DeletecontributionsFromSimchaBySimchaId(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "delete from Contributions where SimchaId=@id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
                
        }

        private void AddContributionsToDB(Contribution C)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "insert into Contributions values (@contributersId,@SimchaId,@amount,@date)";
                command.Parameters.AddWithValue("@contributersId", C.ContributersId);
                command.Parameters.AddWithValue("@SimchaId", C.SimchaId);
                command.Parameters.AddWithValue("@amount", C.Amount);
                if (C.date != default(DateTime))
                {
                    command.Parameters.AddWithValue("@date", C.date);
                }
                else
                {
                    command.Parameters.AddWithValue("@date", DateTime.Now);
                }
                connection.Open();
                command.ExecuteNonQuery();
            }
               
        }
        public DateTime GetDateForContribution(int SimchaId, int contributorId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "	select date from Contributions C where c.contributersId=@contributorsid and c.SimchaId= @SimchaId";
            command.Parameters.AddWithValue("@contributorsid", contributorId);
            command.Parameters.AddWithValue("@SimchaId", SimchaId);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            DateTime date = new DateTime();
            while (reader.Read())
            {
                date = (DateTime)reader["date"];
            }
            return date;
        }

        public List<ContributorsForSimcha> ContributorsListForSImcha(int SimchaID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {

                command.CommandText = "	select * from Contributors";
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<ContributorsForSimcha> Result = new List<ContributorsForSimcha>();
                while (reader.Read())
                {
                    ContributorsForSimcha Contributor = new ContributorsForSimcha();
                    Contributor.Id = (int)reader["id"];
                    Contributor.Name = (string)reader["Name"];
                    Contributor.AlwaysInclude = (bool)reader["AlwaysIncude"];
                    Contributor.Balance = GetBalance((int)reader["id"]);
                    Contributor.Amount = GetAmountPledgedForSimcha(SimchaID, Contributor.Id);
                    Contributor.Date = GetDateForContribution(SimchaID, Contributor.Id);
                    Contributor.PledgedForSimcha = Contributor.Amount != 0 ? true : false;
                    Result.Add(Contributor);
                }
                return Result;
            }
        }
        private int GetAmountPledgedForSimcha(int SimchaId, int PersonId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {

                command.CommandText = "	select * from Contributions C where c.contributersId= @personId and c.SimchaId =@simchaId";
                command.Parameters.AddWithValue("@personId", PersonId);
                command.Parameters.AddWithValue("@simchaId", SimchaId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return (int)(decimal)reader["Amount"];
                }
                else
                {
                    return 0;
                }

            }

        }

    }
}
