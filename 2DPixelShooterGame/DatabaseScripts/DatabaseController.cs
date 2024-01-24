using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
namespace _2DPixelShooterGame.DatabaseScripts
{
    public class DatabaseController
    {
        private DatabaseController() { }
        private static DatabaseController instance;
        private static readonly object _lock = new object();
        public SqlConnection sqlConnection;
        private SqlDataAdapter sqlDataAdapter;
        public DataSet DataSet;
        public static DatabaseController Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new DatabaseController();
                            instance.DataSet = new DataSet();

                        }
                    }
                }
                return instance;
            }
            private set { instance = value; }
        }
        public void GetAllTablesData()
        {
            sqlDataAdapter = new SqlDataAdapter();
            string cmd = "SELECT * FROM PLAYERS;" +
                "SELECT * FROM MAPS;" +
                "SELECT * FROM WEAPONS;" +
                "SELECT * FROM ITEMS;" +
                "SELECT PLAYER_NAME,CREATED_AT,PLAY.* FROM PLAY,PLAYERS WHERE PLAY.PLAYER_ID = PLAYERS.PLAYER_ID;" +
                "SELECT * FROM OWN;" +
                "SELECT * FROM INVENTORY;";
            sqlDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            try
            {
                sqlDataAdapter.SelectCommand = new SqlCommand(cmd, sqlConnection);
                sqlDataAdapter.TableMappings.Add("Table", "PLAYERS");
                sqlDataAdapter.TableMappings.Add("Table1", "MAPS");
                sqlDataAdapter.TableMappings.Add("Table2", "WEAPONS");
                sqlDataAdapter.TableMappings.Add("Table3", "ITEMS");
                sqlDataAdapter.TableMappings.Add("Table4", "PLAY");
                sqlDataAdapter.TableMappings.Add("Table5", "OWN");
                sqlDataAdapter.TableMappings.Add("Table6", "INVENTORY");
                sqlDataAdapter.Fill(this.DataSet);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        public void DoConnect(string Server, string Database, string Trusted, string UID, string PWD)
        {
            var strbuilder = new SqlConnectionStringBuilder();
            strbuilder["Server"] = Server;
            strbuilder["Database"] = Database;
            strbuilder["Trusted_Connection"] = Trusted;
            if (Trusted == "false")
            {
                strbuilder["UID"] = UID;
                strbuilder["PWD"] = PWD;
            }
            this.sqlConnection = new SqlConnection(strbuilder.ToString());
            if (this.sqlConnection.State != ConnectionState.Open)
            {
                DoOpenConnection();
                GetAllTablesData();
            }
        }
        //Players
        public string InsertPlayerData(string Pname)
        {
            string sqlStr = @"INSERT INTO PLAYERS (PLAYER_ID,PLAYER_NAME,HP,AMMO,GOLD,CREATED_AT)
                            SELECT @PID,@PNAME,@HP,@AMMO,@GOLD,@CREATED_AT
                            WHERE NOT EXISTS (
                                SELECT * FROM PLAYERS
                                WHERE PLAYER_ID = @PID
                            )";
            var stt = DataSet.Tables["PLAYERS"].Rows.Count;
            var stt_str = stt < 9 ? ("P0" + (stt + 1).ToString()) : ("P" + (stt + 1).ToString());
            SqlCommand sqlCommand = new SqlCommand(sqlStr, sqlConnection);
            sqlCommand.Parameters.Add("@PID", SqlDbType.VarChar).Value = stt_str;
            sqlCommand.Parameters.Add("@PNAME", SqlDbType.VarChar).Value = Pname;
            sqlCommand.Parameters.Add("@HP", SqlDbType.Int).Value = 100;
            sqlCommand.Parameters.Add("@AMMO", SqlDbType.Int).Value = 0;
            sqlCommand.Parameters.Add("@GOLD", SqlDbType.Int).Value = 0;
            sqlCommand.Parameters.Add("@CREATED_AT", SqlDbType.DateTime).Value = DateTime.Now;
            sqlCommand.ExecuteNonQuery();
            GetAllTablesData();
            return stt_str;
        }
        public List<string> GetPlayerData(string PID)
        {
            var store = new List<string>();
            var tb = this.DataSet.Tables["PLAYERS"];
            tb.PrimaryKey = new DataColumn[] { tb.Columns["PLAYER_ID"] };
            DataRow row = tb.Rows.Find(PID);
            if (row != null)
            {
                foreach (var value in row.ItemArray)
                {
                    store.Add(value.ToString());
                }
                return store;
            }
            return null;
        }
        public string GetPlayerName(string PID)
        {
            var store = new List<string>();
            var tb = this.DataSet.Tables["PLAYERS"];
            tb.PrimaryKey = new DataColumn[] { tb.Columns["PLAYER_ID"] };
            DataRow row = tb.Rows.Find(PID);
            if (row != null)
            {
                return row["PLAYER_NAME"].ToString();
            }
            return "";
        }
        public string GetPlayerCreateTime(string PID)
        {
            var store = new List<string>();
            var tb = this.DataSet.Tables["PLAYERS"];
            tb.PrimaryKey = new DataColumn[] { tb.Columns["PLAYER_ID"] };
            DataRow row = tb.Rows.Find(PID);
            if (row != null)
            {
                return row["CREATED_AT"].ToString();
            }
            return "";
        }
        public void UpdatePlayerHP(string PID, int HP)
        {
            string cmd = "UPDATE PLAYERS SET HP = HP + @HP WHERE PLAYER_ID = @PID";
            var sqlCMD = new SqlCommand(cmd, sqlConnection);
            sqlCMD.Parameters.Add("@PID", SqlDbType.VarChar).Value = PID;
            sqlCMD.Parameters.Add("@HP", SqlDbType.Int).Value = HP;
            sqlCMD.ExecuteNonQuery();
            GetAllTablesData();
        }
        public void UpdatePlayerAmmo(string PID, int Ammo)
        {
            string cmd = "UPDATE PLAYERS SET AMMO = AMMO + @AMMO WHERE PLAYER_ID = @PID";
            var sqlCMD = new SqlCommand(cmd, sqlConnection);
            sqlCMD.Parameters.Add("@PID", SqlDbType.VarChar).Value = PID;
            sqlCMD.Parameters.Add("@AMMO", SqlDbType.Int).Value = Ammo;
            sqlCMD.ExecuteNonQuery();
            GetAllTablesData();
        }
        public void UpdatePlayerGold(string PID, int Gold)
        {
            string cmd = "UPDATE PLAYERS SET GOLD = GOLD + @GOLD WHERE PLAYER_ID = @PID";
            var sqlCMD = new SqlCommand(cmd, sqlConnection);
            sqlCMD.Parameters.Add("@PID", SqlDbType.VarChar).Value = PID;
            sqlCMD.Parameters.Add("@GOLD", SqlDbType.Int).Value = Gold;
            sqlCMD.ExecuteNonQuery();
            GetAllTablesData();
        }
        //Maps
        public string GetMapName(string Map_ID)
        {
            var tb = this.DataSet.Tables["MAPS"];
            tb.PrimaryKey = new DataColumn[] { tb.Columns["MAP_ID"] };
            DataRow row = tb.Rows.Find(Map_ID);
            if (row != null)
            {
                return row[1].ToString();
            }
            return "";
        }
        //Weapons
        public string GetCurrentWeaponID(string Player_ID)
        {
            var tb = this.DataSet.Tables["OWN"];
            tb.PrimaryKey = new DataColumn[] { tb.Columns["PLAYER_ID"] };
            DataRow row = tb.Rows.Find(Player_ID);
            if (row != null)
            {
                return row[1].ToString();
            }
            return "";
        }
        public string GetNextWeaponID(string CurrentWeapon_ID)
        {
            var tb = this.DataSet.Tables["WEAPONS"];
            var letters = new string(CurrentWeapon_ID.Where(char.IsLetter).ToArray());
            var numbers = new string(CurrentWeapon_ID.Where(char.IsDigit).ToArray());
            if (int.TryParse(numbers, out int number))
            {
                number++;
                return letters + number.ToString("D2");
            }
            return "";
        }
        public string GetWeaponName(string WID)
        {
            var tb = this.DataSet.Tables["WEAPONS"];
            tb.PrimaryKey = new DataColumn[] { tb.Columns["WEAPON_ID"] };
            var row = tb.Rows.Find(WID);
            if (row != null)
                return row["WEAPON_NAME"].ToString(); ;
            return "-";
        }
        public string GetWeaponPrice(string WID)
        {
            var tb = this.DataSet.Tables["WEAPONS"];
            tb.PrimaryKey = new DataColumn[] { tb.Columns["WEAPON_ID"] };
            var row = tb.Rows.Find(GetNextWeaponID(WID));
            if (row != null)
                return row["Price"].ToString();
            return "";
        }
        public int GetWeaponDamage(string WID)
        {
            var tb = this.DataSet.Tables["WEAPONS"];
            tb.PrimaryKey = new DataColumn[] { tb.Columns["WEAPON_ID"] };
            var row = tb.Rows.Find(WID);
            if (row != null)
                return int.Parse(row["Damage"].ToString());
            return 0;
        }
        //Items
        public string GetItemName(string IID)
        {
            var tb = this.DataSet.Tables["ITEMS"];
            tb.PrimaryKey = new DataColumn[] { tb.Columns["ITEM_ID"] };
            var row = tb.Rows.Find(IID);
            if (row != null)
                return row["Item_Name"].ToString();
            return "";
        }
        public string GetItemInfo(string IID)
        {
            var tb = this.DataSet.Tables["ITEMS"];
            tb.PrimaryKey = new DataColumn[] { tb.Columns["ITEM_ID"] };
            var row = tb.Rows.Find(IID);
            if (row != null)
                return row["Item_Info"].ToString();
            return "";
        }
        //Play
        public void UpdatePlayData(string PID, string MID, int STAGE, int DIFFICULTY, char ISVICTORY, int ATTEMPT)
        {
            string cmd = "IF NOT EXISTS (SELECT * FROM PLAY WHERE PLAYER_ID = @PID)" +
                "BEGIN\n" +
                "INSERT INTO PLAY VALUES (@PID,@MID,@STAGE,@DIFF,@RES,@ATTEMPT)\n" +
                "END\n" +
                "ELSE\n" +
                "BEGIN\n" +
                "UPDATE PLAY SET STAGE=@STAGE, DIFFICULTY=@DIFF, ISVICTORY=@RES, ATTEMPT =@ATTEMPT WHERE PLAYER_ID = @PID\n" +
                "END";
            var sqlCMD = new SqlCommand(cmd, sqlConnection);
            sqlCMD.Parameters.Add("@PID", SqlDbType.VarChar).Value = PID;
            sqlCMD.Parameters.Add("@MID", SqlDbType.VarChar).Value = MID;
            sqlCMD.Parameters.Add("@STAGE", SqlDbType.Int).Value = STAGE;
            sqlCMD.Parameters.Add("@DIFF", SqlDbType.Int).Value = DIFFICULTY;
            sqlCMD.Parameters.Add("@RES", SqlDbType.Char).Value = ISVICTORY;
            sqlCMD.Parameters.Add("@ATTEMPT", SqlDbType.Int).Value = ATTEMPT;
            sqlCMD.ExecuteNonQuery();
            GetAllTablesData();
        }
        public void UpdatePlayStage(string PID, string MID, int STAGE)
        {
            string cmd = "UPDATE PLAY SET STAGE= @STAGE WHERE PLAYER_ID = @PID AND MAP_ID = @MID";
            var sqlCMD = new SqlCommand(cmd, sqlConnection);
            sqlCMD.Parameters.Add("@PID", SqlDbType.VarChar).Value = PID;
            sqlCMD.Parameters.Add("@MID", SqlDbType.VarChar).Value = MID;
            sqlCMD.Parameters.Add("@STAGE", SqlDbType.Int).Value = STAGE;
            sqlCMD.ExecuteNonQuery();
            GetAllTablesData();
        }
        public void UpdatePlayAttempt(string PID,string MID,int ATTEMPT)
        {
            string cmd = "UPDATE PLAY SET ATTEMPT= @ATTEMPT WHERE PLAYER_ID = @PID AND MAP_ID = @MID";
            var sqlCMD = new SqlCommand(cmd, sqlConnection);
            sqlCMD.Parameters.Add("@PID", SqlDbType.VarChar).Value = PID;
            sqlCMD.Parameters.Add("@MID", SqlDbType.VarChar).Value = MID;
            sqlCMD.Parameters.Add("@ATTEMPT", SqlDbType.Int).Value = ATTEMPT;
            sqlCMD.ExecuteNonQuery();
            GetAllTablesData();
        }

        public void UpdatePlayIsVictory(string PID, string MID, char isvictory)
        {
            string cmd = "UPDATE PLAY SET ISVICTORY= @VICTORY WHERE PLAYER_ID = @PID AND MAP_ID = @MID";
            var sqlCMD = new SqlCommand(cmd, sqlConnection);
            sqlCMD.Parameters.Add("@PID", SqlDbType.VarChar).Value = PID;
            sqlCMD.Parameters.Add("@MID", SqlDbType.VarChar).Value = MID;
            sqlCMD.Parameters.Add("@VICTORY", SqlDbType.Char).Value = isvictory;
            sqlCMD.ExecuteNonQuery();
            GetAllTablesData();
        }
        //Own
        public void UpdateOwnData(string PID, string WID)
        {
            string cmd = "IF NOT EXISTS (SELECT * FROM OWN WHERE PLAYER_ID = @PID)" +
                "BEGIN\n" +
                "INSERT INTO OWn VALUES (@PID,@WID)\n" +
                "END\n" +
                "ELSE\n" +
                "BEGIN\n" +
                "UPDATE OWN SET WEAPON_ID = @WID WHERE PLAYER_ID = @PID\n" +
                "END";
            var sqlCMD = new SqlCommand(cmd, sqlConnection);
            sqlCMD.Parameters.Add("@PID", SqlDbType.VarChar).Value = PID;
            sqlCMD.Parameters.Add("@WID", SqlDbType.VarChar).Value = WID;
            sqlCMD.ExecuteNonQuery();
            GetAllTablesData();
        }
        //Inventory
        public void UpdateInventory(string PID, string IID, int Quantity)
        {
            string cmd = "IF NOT EXISTS (SELECT * FROM INVENTORY WHERE PLAYER_ID = @PID AND ITEM_ID = @IID)" +
               "BEGIN\n" +
               "INSERT INTO INVENTORY VALUES (@PID,@IID,@Q)\n" +
               "END\n" +
               "ELSE\n" +
               "BEGIN\n" +
               "UPDATE INVENTORY SET QUANTITY = QUANTITY + @Q WHERE PLAYER_ID = @PID AND ITEM_ID = @IID\n" +
               "END";
            var sqlCMD = new SqlCommand(cmd, sqlConnection);
            sqlCMD.Parameters.Add("@PID", SqlDbType.VarChar).Value = PID;
            sqlCMD.Parameters.Add("@IID", SqlDbType.VarChar).Value = IID;
            sqlCMD.Parameters.Add("@Q", SqlDbType.Int).Value = Quantity;
            sqlCMD.ExecuteNonQuery();
            GetAllTablesData();
        }
        public void DoOpenConnection()
        {
            this.sqlConnection.Open();
        }
        public void DoCloseConnection()
        {
            this.sqlConnection.Close();
        }

    }
}
