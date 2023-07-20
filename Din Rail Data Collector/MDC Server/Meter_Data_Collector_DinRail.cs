using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading;
using System.Data.Odbc;
using System.Globalization;
using System.Timers;

namespace MDC
{
    public partial class Meter_Data_Collector_DinRail : Form
    {

        private SerialPort sPort = new SerialPort();
        byte[] reader = new byte[9];
        string demand;
        int m = 0;
        string myconn = "DRIVER={MySQL ODBC 3.51 Driver};Database=dingrail;Server=localhost;Port=3306;UID=root;PWD=12345;";
        DateTime DT = DateTime.Now;
        bool Valid_MSN = false;
        //*******************************************//
        //***********Variables Declerations**********//
        //*******************************************//
        float TotalActive_Energy = 0f;
        float TotalReactive_Energy = 0f;
        float A_phaseandN_voltage = 0f;
        float B_phaseandN_voltage = 0f;
        float C_phaseandN_voltage = 0f;
        float Current_a = 0f;
        float Current_b = 0f;
        float Current_c = 0f;
        float Current_n = 0f;
        float combined_active_power = 0f;
        float combined_reactive_power = 0f;
        float combined_Frequency = 0f;
        float combined_power_factor = 0f;
        float combined_active_demand = 0f;
        //*******************************************//
        //*******************************************//
        public Meter_Data_Collector_DinRail()
        {
            InitializeComponent();
            label1.Visible=false;

            string[] portNames = SerialPort.GetPortNames();
            Array.Sort<string>(portNames);
            this.serialPort_cmb.Items.Clear();
            this.serialPort_cmb.Items.AddRange((object[])portNames);
            if (this.serialPort_cmb.Items.Count >= 1)
            {
                this.serialPort_cmb.SelectedIndex = 0;
            }

        }
        public void RecordLoadProfile()
        {
            System.Timers.Timer timer = new System.Timers.Timer(30 * 60 * 1000);
            timer.Elapsed += OnTimerElapsed;
            timer.Start();
        }
        private void ExecuteNonQurey(string query)
        {
            DBGetSet db = new DBGetSet();
            db.Query = query;
            db.ExecuteNonQuery();
        }

        private DataTable ExecuteReader(string query)
        {
            DBGetSet db = new DBGetSet();
            db.Query = query;
            return db.ExecuteReader();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            ExecuteNonQurey("UPDATE `dingrail`.`meter` SET demand ='1';");
        }
       

        private void HandleClient()
        {
            do
            {
                try
                {
                    RecordLoadProfile();
                    DataTable dt = new DataTable();
                    string MSN = null;
                    int bytes_read = 0;
                    byte num = 0;
                    byte[] n;
                    string str = "";
                    string[] result;
                    bool flagger = false;
                    Console.WriteLine("Performing jobs on devices...");
                    dt = ExecuteReader("SELECT * FROM dingrail.meter;");
                    foreach (DataRow dr in dt.Rows)
                    {
                        MSN = ((int)dr["subsubsubutility"]).ToString();
                        demand = ((int)dr["demand"]).ToString();
                        
                        if (MSN != null)
                        {
                            Valid_MSN = true;
                            
                        }
                        if (Valid_MSN == true)
                        {
                            int intValue = int.Parse(demand);
                            Console.WriteLine("Performing job on {MSN}..." + MSN);
                            flagger = false;
                            ExecuteNonQurey("UPDATE `dingrail`.`meter` SET connected ='1' WHERE subsubsubutility ='" + MSN + "';");
                            ExecuteNonQurey("UPDATE `dingrail`.`meter` SET reading ='1' WHERE subsubsubutility ='" + MSN + "';");
                            ExecuteNonQurey("UPDATE `dingrail`.`meter` SET try ='0' WHERE subsubsubutility ='" + MSN + "';");
                            ExecuteNonQurey("UPDATE `dingrail`.`meter` SET retry ='0' WHERE subsubsubutility ='" + MSN + "';");
                            ExecuteNonQurey("UPDATE `dingrail`.`meter` SET demand ='0' WHERE subsubsubutility ='" + MSN + "';");
                            n = Encoding.UTF8.GetBytes(MSN);
                            for (int i = 0; i < n.Length; i++)
                            {
                                num = (byte)(n[i] - 0x30);
                            }

                            try
                            {
                                // Define a jagged array of bytes called 'data' that 
                                //contains multiple byte arrays with different values
                                byte[][] data = new byte[][] {
                                    new byte[] {num,0x04,0x01,0x22,0x00,0x02},
                                    new byte[] {num,0x04,0x01,0x40,0x00,0x02},
                                    new byte[] {num,0x04,0x04,0x00,0x00,0x02},
                                    new byte[] {num,0x04,0x04,0x02,0x00,0x02},
                                    new byte[] {num,0x04,0x04,0x04,0x00,0x02},
                                    new byte[] {num,0x04,0x04,0x10,0x00,0x02},
                                    new byte[] {num,0x04,0x04,0x12,0x00,0x02},
                                    new byte[] {num,0x04,0x04,0x14,0x00,0x02},
                                    new byte[] {num,0x04,0x04,0x16,0x00,0x02},
                                    new byte[] {num,0x04,0x04,0x20,0x00,0x02},
                                    new byte[] {num,0x04,0x04,0x30,0x00,0x02},
                                    new byte[] {num,0x04,0x04,0x35,0x00,0x01},
                                    new byte[] {num,0x04,0x04,0x39,0x00,0x01},
                                    new byte[] {num,0x04,0x04,0x50,0x00,0x02},
                                };


                                // Define a string array called 'result' to store the resulting strings with CRC values added
                                result = null;
                                result = new string[data.Length];

                                // Iterate over each byte array in 'data'
                                for (int i = 0; i < data.Length; i++)
                                {
                                    // Convert the current byte array to a string and replace "-" with " " for readability
                                    string s = BitConverter.ToString(data[i]).Replace("-", " ");
                                    // Calculate the ModRTU CRC value for the current byte array
                                    UInt16 b = ModRTU_CRC(data[i], data[i].Length);
                                    // Convert the CRC value to a hex string
                                    string hexValue = b.ToString("X");
                                    // Check if the hex string has an 
                                    //odd number of characters and add a leading "0" if necessary
                                    hexValue = check(hexValue);
                                    // Rotate the hex string 
                                    //with spaces to group the characters into pairs
                                    hexValue = RotateWithSpaces(hexValue, 2);
                                    // Concatenate the original byte array 
                                    //string and the CRC value string with spaces in between
                                    result[i] = s + " " + hexValue;
                                }
                                str = "";
                                //---------------------//
                                //Total_Active_Energy//
                                //---------------------//
                                this.sPort.Open();

                                try
                                {
                                    sPort.ReadTimeout = 1000;
                                    byte[] Total_Active_Energy = (result[0]).Split().Select(s => Convert.ToByte(s, 16)).ToArray();
                                    sPort.Write(Total_Active_Energy, 0, Total_Active_Energy.Length);

                                    for (int index = 0; index < Total_Active_Energy.Length; ++index)
                                    {
                                        str = str + Total_Active_Energy[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Total Active Energy Command:  Sent:  " + str);

                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Total Active Energy Command Send" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");
                                    Thread.Sleep(100);
                                    bytes_read = sPort.Read(reader, 0, reader.Length);



                                    m = reader[bytes_read - 9];
                                    TotalActive_Energy = calculator(bytes_read, reader);
                                    str = "";
                                    for (int index = 0; index < bytes_read; ++index)
                                    {
                                        str = str + reader[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Total Active Energy Command Recieved:  " + str + "   " + "=>" + "Total Active Energy = " + TotalActive_Energy);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Total Active Energy Command Reply Recieved" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    //---------------------//
                                    //Total Reactive Energy//
                                    //---------------------//
                                    Array.Clear(reader, 0, reader.Length);
                                    str = "";
                                    Thread.Sleep(100);
                                    byte[] Total_Reactive_Energy = (result[1]).Split().Select(s => Convert.ToByte(s, 16)).ToArray();
                                    sPort.Write(Total_Reactive_Energy, 0, Total_Reactive_Energy.Length);

                                    for (int index = 0; index < Total_Reactive_Energy.Length; ++index)
                                    {
                                        str = str + Total_Reactive_Energy[index].ToString("X2") + " ";
                                    }
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Total Reactive Energy Command Send" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    Console.WriteLine(this.DT.ToString() + "  Total Reactive Energy Command:  Sent:  " + str);
                                    Thread.Sleep(100);
                                    bytes_read = sPort.Read(reader, 0, reader.Length);
                                    TotalReactive_Energy = calculator(bytes_read, reader);
                                    str = "";
                                    for (int index = 0; index < bytes_read; ++index)
                                    {
                                        str = str + reader[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Total Reactive Energy Command Recieved:  " + str + "   " + "=>" + "Total Reactive Energy = " + TotalReactive_Energy);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Total Reactive Energy Command Reply Recieved" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    //---------------------//
                                    //A_phase_and_N_voltage//
                                    //---------------------//
                                    Array.Clear(reader, 0, reader.Length);
                                    str = "";
                                    Thread.Sleep(100);
                                    byte[] A_phase_and_N_voltage = (result[2]).Split().Select(s => Convert.ToByte(s, 16)).ToArray();
                                    sPort.Write(A_phase_and_N_voltage, 0, A_phase_and_N_voltage.Length);

                                    for (int index = 0; index < A_phase_and_N_voltage.Length; ++index)
                                    {
                                        str = str + A_phase_and_N_voltage[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  A phase and N voltage:  Command Sent:  " + str);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Voltage at phase A Command Send" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    Thread.Sleep(100);
                                    bytes_read = sPort.Read(reader, 0, reader.Length);
                                    A_phaseandN_voltage = calculator(bytes_read, reader);
                                    str = "";
                                    for (int index = 0; index < bytes_read; ++index)
                                    {
                                        str = str + reader[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  A phase and N voltage: Command Recieved:  " + str + "   " + "=>" + "A phase and N voltage = " + A_phaseandN_voltage);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Voltage at phase A Command Recieved" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    //---------------------//
                                    //B_phase_and_N_voltage//
                                    //---------------------//
                                    Array.Clear(reader, 0, reader.Length);
                                    str = "";
                                    Thread.Sleep(100);
                                    byte[] B_phase_and_N_voltage = (result[3]).Split().Select(s => Convert.ToByte(s, 16)).ToArray();
                                    sPort.Write(B_phase_and_N_voltage, 0, B_phase_and_N_voltage.Length);

                                    for (int index = 0; index < B_phase_and_N_voltage.Length; ++index)
                                    {
                                        str = str + B_phase_and_N_voltage[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  B phase and N voltage:  Command Sent:  " + str);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Voltage at phase B Command Send" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    Thread.Sleep(100);
                                    bytes_read = sPort.Read(reader, 0, reader.Length);
                                    B_phaseandN_voltage = calculator(bytes_read, reader);
                                    str = "";
                                    for (int index = 0; index < bytes_read; ++index)
                                    {
                                        str = str + reader[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  B phase and N voltage: Command Recieved:  " + str + "   " + "=>" + "B phase and N voltage = " + B_phaseandN_voltage);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Voltage at phase B Command Recieved" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    //---------------------//
                                    //C_phase_and_N_voltage//
                                    //---------------------//

                                    Array.Clear(reader, 0, reader.Length);
                                    str = "";
                                    Thread.Sleep(100);
                                    byte[] C_phase_and_N_voltage = (result[4]).Split().Select(s => Convert.ToByte(s, 16)).ToArray();
                                    sPort.Write(C_phase_and_N_voltage, 0, C_phase_and_N_voltage.Length);

                                    for (int index = 0; index < C_phase_and_N_voltage.Length; ++index)
                                    {
                                        str = str + C_phase_and_N_voltage[index].ToString("X2") + " ";
                                    }
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Voltage at phase C Command Send" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    Console.WriteLine(this.DT.ToString() + "  C phase and N voltage:  Command Sent:  " + str);
                                    Thread.Sleep(100);
                                    bytes_read = sPort.Read(reader, 0, reader.Length);
                                    C_phaseandN_voltage = calculator(bytes_read, reader);
                                    str = "";
                                    for (int index = 0; index < bytes_read; ++index)
                                    {
                                        str = str + reader[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  C phase and N voltage: Command Recieved:  " + str + "   " + "=>" + "C phase and N voltage = " + C_phaseandN_voltage);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Voltage at phase C Command Recieved" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    //---------------------//
                                    //Current A//
                                    //---------------------//
                                    Array.Clear(reader, 0, reader.Length);
                                    str = "";
                                    Thread.Sleep(100);
                                    byte[] Current_A = (result[5]).Split().Select(s => Convert.ToByte(s, 16)).ToArray();
                                    sPort.Write(Current_A, 0, Current_A.Length);

                                    for (int index = 0; index < Current_A.Length; ++index)
                                    {
                                        str = str + Current_A[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Current_A:  Command Sent:  " + str);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Current at phase A Command Send" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    Thread.Sleep(100);
                                    bytes_read = sPort.Read(reader, 0, reader.Length);
                                    Current_a = calculator(bytes_read, reader);
                                    str = "";
                                    for (int index = 0; index < bytes_read; ++index)
                                    {
                                        str = str + reader[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Current A: Command Recieved:  " + str + "   " + "=>" + "Current A = " + Current_a);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Current at phase A Command Recieved" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    //---------------------//
                                    //Current B//
                                    //---------------------//
                                    Array.Clear(reader, 0, reader.Length);
                                    str = "";
                                    Thread.Sleep(100);
                                    byte[] Current_B = (result[6]).Split().Select(s => Convert.ToByte(s, 16)).ToArray();
                                    sPort.Write(Current_B, 0, Current_B.Length);

                                    for (int index = 0; index < Current_B.Length; ++index)
                                    {
                                        str = str + Current_B[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Current_B:  Command Sent:  " + str);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Current at phase B Command Send" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    Thread.Sleep(100);
                                    bytes_read = sPort.Read(reader, 0, reader.Length);
                                    Current_b = calculator(bytes_read, reader);
                                    str = "";
                                    for (int index = 0; index < bytes_read; ++index)
                                    {
                                        str = str + reader[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Current B: Command Recieved:  " + str + "   " + "=>" + "Current B = " + Current_b);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Current at phase B Command Recieved" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    //---------------------//
                                    //Current C//
                                    //---------------------//
                                    Array.Clear(reader, 0, reader.Length);
                                    str = "";
                                    Thread.Sleep(100);
                                    byte[] Current_C = (result[7]).Split().Select(s => Convert.ToByte(s, 16)).ToArray();
                                    sPort.Write(Current_C, 0, Current_C.Length);

                                    for (int index = 0; index < Current_C.Length; ++index)
                                    {
                                        str = str + Current_C[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Current C:  Command Sent:  " + str);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Current at phase C Command Send" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    Thread.Sleep(100);
                                    bytes_read = sPort.Read(reader, 0, reader.Length);
                                    Current_c = calculator(bytes_read, reader);
                                    str = "";
                                    for (int index = 0; index < bytes_read; ++index)
                                    {
                                        str = str + reader[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Current C: Command Recieved:  " + str + "   " + "=>" + "Current C = " + Current_c);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Current at phase C Command Recieved" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    //---------------------//
                                    //Current N//
                                    //---------------------//
                                    Array.Clear(reader, 0, reader.Length);
                                    str = "";
                                    Thread.Sleep(100);
                                    byte[] Current_N = (result[8]).Split().Select(s => Convert.ToByte(s, 16)).ToArray();
                                    sPort.Write(Current_N, 0, Current_N.Length);

                                    for (int index = 0; index < Current_N.Length; ++index)
                                    {
                                        str = str + Current_N[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Current N:  Command Sent:  " + str);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Current at phase N Command Send" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    Thread.Sleep(100);
                                    bytes_read = sPort.Read(reader, 0, reader.Length);
                                    Current_n = calculator(bytes_read, reader);
                                    str = "";
                                    for (int index = 0; index < bytes_read; ++index)
                                    {
                                        str = str + reader[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Current N: Command Recieved:  " + str + "   " + "=>" + "Current N = " + Current_n);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Current at phase N Command Recieved" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    //---------------------//
                                    //Combined_Active_POWER//
                                    //---------------------// 
                                    Array.Clear(reader, 0, reader.Length);
                                    str = "";
                                    Thread.Sleep(100);
                                    byte[] Combined_Active_POWER = (result[9]).Split().Select(s => Convert.ToByte(s, 16)).ToArray();
                                    sPort.Write(Combined_Active_POWER, 0, Combined_Active_POWER.Length);

                                    for (int index = 0; index < Combined_Active_POWER.Length; ++index)
                                    {
                                        str = str + Combined_Active_POWER[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Combined Active POWER:  Command Sent:  " + str);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Combined Active POWER Command Send" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    Thread.Sleep(100);
                                    bytes_read = sPort.Read(reader, 0, reader.Length);
                                    combined_active_power = calculator(bytes_read, reader);
                                    str = "";
                                    for (int index = 0; index < bytes_read; ++index)
                                    {
                                        str = str + reader[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Combined Active POWER: Command Recieved:  " + str + "   " + "=>" + "Combined Active Power = " + combined_active_power);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Combined Active POWER Command Recieved" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    //---------------------//
                                    //Combined_Reactive_POWER//
                                    //---------------------// 
                                    Array.Clear(reader, 0, reader.Length);
                                    str = "";
                                    Thread.Sleep(100);
                                    byte[] Combined_Reactive_Power = (result[10]).Split().Select(s => Convert.ToByte(s, 16)).ToArray();
                                    sPort.Write(Combined_Reactive_Power, 0, Combined_Reactive_Power.Length);

                                    for (int index = 0; index < Combined_Reactive_Power.Length; ++index)
                                    {
                                        str = str + Combined_Reactive_Power[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Combined Reactive Power:  Command Sent:  " + str);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Combined Reactive POWER Command Send" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    Thread.Sleep(100);
                                    bytes_read = sPort.Read(reader, 0, reader.Length);
                                    combined_reactive_power = calculator(bytes_read, reader);
                                    str = "";
                                    for (int index = 0; index < bytes_read; ++index)
                                    {
                                        str = str + reader[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Combined Reactive POWER: Command Recieved:  " + str + "   " + "=>" + "Combined Reactive power = " + combined_reactive_power);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Combined Reactive POWER Command Recieved" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");
                                    //---------------------//
                                    //Combined_Frequency//
                                    //---------------------// 
                                    Array.Clear(reader, 0, reader.Length);
                                    str = "";
                                    Thread.Sleep(100);
                                    byte[] Combined_Frequency = (result[11]).Split().Select(s => Convert.ToByte(s, 16)).ToArray();
                                    sPort.Write(Combined_Frequency, 0, Combined_Frequency.Length);

                                    for (int index = 0; index < Combined_Frequency.Length; ++index)
                                    {
                                        str = str + Combined_Frequency[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Combined_Frequency:  Command Sent:  " + str);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Combined Frequency Command Send" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    Thread.Sleep(100);
                                    bytes_read = sPort.Read(reader, 0, reader.Length);
                                    combined_Frequency = (UInt32)((reader[bytes_read - 4] << 8) | (reader[bytes_read - 3] << 0));
                                    combined_Frequency = combined_Frequency / 10;
                                    str = "";
                                    for (int index = 0; index < bytes_read; ++index)
                                    {
                                        str = str + reader[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Combined Frequency: Command Recieved:  " + str + "   " + "=>" + "Combined Frequency = " + combined_Frequency);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Combined Frequency Command Recieved" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    //---------------------//
                                    //Combined_Power_Factor//
                                    //---------------------// 

                                    Array.Clear(reader, 0, reader.Length);
                                    str = "";
                                    Thread.Sleep(100);
                                    byte[] Combined_Power_Factor = (result[12]).Split().Select(s => Convert.ToByte(s, 16)).ToArray();
                                    sPort.Write(Combined_Power_Factor, 0, Combined_Power_Factor.Length);

                                    for (int index = 0; index < Combined_Power_Factor.Length; ++index)
                                    {
                                        str = str + Combined_Power_Factor[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Combined Power Factor:  Command Sent:  " + str);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Combined Power Factor Command Send" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");
                                    Thread.Sleep(100);
                                    bytes_read = sPort.Read(reader, 0, reader.Length);
                                    combined_power_factor = (UInt32)((reader[bytes_read - 4] << 8) | (reader[bytes_read - 3] << 0));
                                    combined_power_factor = combined_power_factor / 1000;
                                    str = "";
                                    for (int index = 0; index < bytes_read; ++index)
                                    {
                                        str = str + reader[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Combined Power Factor: Command Recieved:  " + str + "   " + "=>" + "Combined Power Factor = " + combined_power_factor);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Combined Power Factor Command Recieved" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    //---------------------//
                                    //Combined_Active_demand//
                                    //---------------------// 
                                    Array.Clear(reader, 0, reader.Length);
                                    str = "";
                                    Thread.Sleep(100);
                                    byte[] Combined_Active_demand = (result[13]).Split().Select(s => Convert.ToByte(s, 16)).ToArray();
                                    sPort.Write(Combined_Active_demand, 0, Combined_Active_demand.Length);

                                    for (int index = 0; index < Combined_Active_demand.Length; ++index)
                                    {
                                        str = str + Combined_Active_demand[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Combined Active Demand:  Command Sent:  " + str);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Combined Active Command Send" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");

                                    Thread.Sleep(100);
                                    bytes_read = sPort.Read(reader, 0, reader.Length);
                                    combined_active_demand = calculator(bytes_read, reader);
                                    str = "";
                                    for (int index = 0; index < bytes_read; ++index)
                                    {
                                        str = str + reader[index].ToString("X2") + " ";
                                    }
                                    Console.WriteLine(this.DT.ToString() + "  Combined Active demand: Command Recieved:  " + str + "   " + "=>" + "Combined Active Demand = " + combined_active_demand);
                                    ExecuteNonQurey("INSERT INTO dingrail.communicationlog(serial,action,data,Time_stamp) VALUES(" + m + ", '" + "Combined Active Command Recieved" + "', '" + str + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");
                                }
                                catch (TimeoutException ex)
                                {
                                    Console.Write(ex.ToString());
                                    flagger = true;
                                    
                                }
                                try
                                {
                                    if (flagger == false)
                                    {
                                        if (intValue == 1)
                                        {
                                            ExecuteNonQurey("INSERT INTO dingrail.loadprofile (serial,kWh,kW,kVARh,kVAR,Time_stamp) values('" + m + "','" + TotalActive_Energy + "','" + combined_active_power + "','" + TotalReactive_Energy + "','" + combined_reactive_power + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');");
                                        }
                                        ExecuteNonQurey("INSERT INTO `dingrail`.`meter_data` (meter_no,total_active_energy,total_reactive_energy,a_phase_and_n_voltage,b_phase_and_n_voltage, c_phase_and_n_voltage, current_a, current_b, current_c, combine_active_power, combine_reactive_power, combine_frequency, combine_power_factor, combine_active_demand,Time_stamp) VALUES(" + m + ", '" + TotalActive_Energy + "', '" + TotalReactive_Energy + "', '" + A_phaseandN_voltage + "','" + B_phaseandN_voltage + "','" + C_phaseandN_voltage + "','" + Current_a + "','" + Current_b + "','" + Current_c + "','" + combined_active_power + "','" + combined_reactive_power + "','" + combined_Frequency + "','" + combined_power_factor + "','" + combined_active_demand + "','" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "');");
                                        ExecuteNonQurey("INSERT INTO dingrail.systempower(meter_serial, power, Time_stamp, rpower) VALUES (" + m + ", '" + combined_active_power + "', '" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "', '" + combined_reactive_power + "');");

                                        Console.WriteLine("------------------------------");
                                        Graphical_data(m);
                                        ExecuteNonQurey("UPDATE `dingrail`.`meter` SET connected ='0' WHERE subsubsubutility ='" + MSN + "';");
                                        ExecuteNonQurey("UPDATE `dingrail`.`meter` SET reading ='1' WHERE subsubsubutility ='" + MSN + "';");
                                        ExecuteNonQurey("UPDATE `dingrail`.`meter` SET lastRead ='" + String.Format("{0:yyyy/M/d HH:mm:ss}", DateTime.Now) + "' WHERE subsubsubutility ='" + MSN + "';");
                                        ExecuteNonQurey("UPDATE `dingrail`.`meter` SET demand ='1' WHERE subsubsubutility ='" + MSN + "';");
                                        ExecuteNonQurey("UPDATE `dingrail`.`meter` SET instantaneous ='1' WHERE subsubsubutility ='" + MSN + "';");

                                        Console.WriteLine("EndCommunication with Meter Number =" + MSN);
                                        Console.WriteLine("------------------------------");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.Write(ex.ToString());
                                    ExecuteNonQurey("UPDATE `dingrail`.`meter` SET connected ='0' WHERE subsubsubutility ='" + MSN + "';");
                                    ExecuteNonQurey("UPDATE `dingrail`.`meter` SET try ='1' WHERE subsubsubutility ='" + MSN + "';");

                                }
                            }
                            catch (Exception ex)
                            {
                                 Console.Write(ex.ToString());
                                ExecuteNonQurey("UPDATE `dingrail`.`meter` SET retry ='1' WHERE subsubsubutility ='" + MSN + "';");

                            }

                            
                            Console.WriteLine("Job completed on Meter =" + MSN);
                            this.sPort.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                    this.sPort.Close();
                }
            }
            while (true);

        }

        static UInt16 ModRTU_CRC(byte[] buf, int len)
        {
            UInt16 crc = 0xFFFF;
            for (int pos = 0; pos < len; pos++)
            {
                crc ^= (UInt16)buf[pos]; // XOR byte into least sig. byte of crc
                for (int i = 8; i != 0; i--)
                { // Loop over each bit
                    if ((crc & 0x0001) != 0)
                    { // If the LSB is set
                        crc >>= 1; // Shift right and XOR 0xA001
                        crc ^= 0xA001;
                    }
                    else // Else LSB is not set
                        crc >>= 1; // Just shift right
                }
            } // Note, this number has low and high bytes swapped, so use it accordingly (or swap bytes)
            return crc;
        }
        void Graphical_data(int meter_no)
        {
            using (OdbcConnection connection = new OdbcConnection(myconn))
            {
                connection.Open();

                using (OdbcCommand command = new OdbcCommand("INSERT INTO info1 (msn,OBIS,Amount,unit,Time_stamp,quantity) VALUES ('" + meter_no + "','1.0.0.8.0.255','" + TotalActive_Energy + "','kWh','" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "', 1);", connection))
                    command.ExecuteNonQuery();

                using (OdbcCommand command = new OdbcCommand("INSERT INTO info1 (msn,OBIS,Amount,unit,Time_stamp,quantity) VALUES ('" + meter_no + "','1.0.94.92.0.255','" + TotalReactive_Energy + "','kVARh','" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "', 1);", connection))
                    command.ExecuteNonQuery();

                using (OdbcCommand command = new OdbcCommand("INSERT INTO info1 (msn,OBIS,Amount,unit,Time_stamp,quantity) VALUES ('" + meter_no + "','1.0.32.7.0.255','" + A_phaseandN_voltage + "','V','" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "', 1);", connection))
                    command.ExecuteNonQuery();

                using (OdbcCommand command = new OdbcCommand("INSERT INTO info1 (msn,OBIS,Amount,unit,Time_stamp,quantity) VALUES ('" + meter_no + "','1.0.52.7.0.255','" + B_phaseandN_voltage + "','V','" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "', 1);", connection))
                    command.ExecuteNonQuery();

                using (OdbcCommand command = new OdbcCommand("INSERT INTO info1 (msn,OBIS,Amount,unit,Time_stamp,quantity) VALUES ('" + meter_no + "','1.0.72.7.0.255','" + C_phaseandN_voltage + "','V','" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "', 1);", connection))
                    command.ExecuteNonQuery();

                using (OdbcCommand command = new OdbcCommand("INSERT INTO info1 (msn,OBIS,Amount,unit,Time_stamp,quantity) VALUES ('" + meter_no + "','1.0.31.7.0.255','" + Current_a + "','A','" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "', 1);", connection))
                    command.ExecuteNonQuery();

                using (OdbcCommand command = new OdbcCommand("INSERT INTO info1 (msn,OBIS,Amount,unit,Time_stamp,quantity) VALUES ('" + meter_no + "','1.0.51.7.0.255','" + Current_b + "','A','" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "', 1);", connection))
                    command.ExecuteNonQuery();

                using (OdbcCommand command = new OdbcCommand("INSERT INTO info1 (msn,OBIS,Amount,unit,Time_stamp,quantity) VALUES ('" + meter_no + "','1.0.71.7.0.255','" + Current_c + "','A','" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "', 1);", connection))
                    command.ExecuteNonQuery();

                using (OdbcCommand command = new OdbcCommand("INSERT INTO info1 (msn,OBIS,Amount,unit,Time_stamp,quantity) VALUES ('" + meter_no + "','1.0.72.8.0.255','" + Current_n + "','A','" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "', 1);", connection))
                    command.ExecuteNonQuery();

                using (OdbcCommand command = new OdbcCommand("INSERT INTO info1 (msn,OBIS,Amount,unit,Time_stamp,quantity) VALUES ('" + meter_no + "','1.0.15.6.0.255','" + combined_active_power + "','kW','" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "', 1);", connection))
                    command.ExecuteNonQuery();

                using (OdbcCommand command = new OdbcCommand("INSERT INTO info1 (msn,OBIS,Amount,unit,Time_stamp,quantity) VALUES ('" + meter_no + "','1.0.3.7.0.255','" + combined_reactive_power + "','kVAR','" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "', 1);", connection))
                    command.ExecuteNonQuery();

                using (OdbcCommand command = new OdbcCommand("INSERT INTO info1 (msn,OBIS,Amount,unit,Time_stamp,quantity) VALUES ('" + meter_no + "','1.0.14.7.0.255','" + combined_Frequency + "','Hz','" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "', 1);", connection))
                    command.ExecuteNonQuery();

                using (OdbcCommand command = new OdbcCommand("INSERT INTO info1 (msn,OBIS,Amount,unit,Time_stamp,quantity) VALUES ('" + meter_no + "','1.0.13.7.0.255','" + combined_power_factor + "','N/A','" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "', 1);", connection))
                    command.ExecuteNonQuery();

                using (OdbcCommand command = new OdbcCommand("INSERT INTO info1 (msn,OBIS,Amount,unit,Time_stamp,quantity) VALUES ('" + meter_no + "','1.0.13.20.0.255','" + combined_active_demand + "','kW','" + DateTime.Now.ToString("yyyy/M/d HH:mm:ss") + "', 1);", connection))
                    command.ExecuteNonQuery();

                

                connection.Close();
            }
        }

        float calculator(int bytes_read, byte[] reader)
        {
            float calculations = (UInt32)((reader[bytes_read - 6] << 24) | (reader[bytes_read - 5] << 16) | (reader[bytes_read - 4] << 8) | (reader[bytes_read - 3] << 0));
            calculations = calculations / 1000;
            return calculations;
        }
        public string RotateWithSpaces(string s, int n)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }
            n %= s.Length;
            string left = s.Substring(n);
            string right = s.Substring(0, n);
            return left + " " + right;
        }
        string check(string hex)
        {
            int n = hex.Length;
            if (n < 4)
            {
                hex = "0" + hex;
            }
            return hex;
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.sPort.IsOpen)
                {
                    this.sPort.Close();
                    label1.Text = "Serial Port is closed";
                    label1.Show();
                }
                else
                {
                    label1.Text = "No Port is Connected Yet";
                    label1.Show();
                }
            }
            catch (Exception ex)
            {
                var LineNumber = new StackTrace(ex, true).GetFrame(0).GetFileLineNumber();
                MessageBox.Show("Line #: " + LineNumber + " - " + ex.Message);
            }
        }
        private void btnClientService_Click(object sender, EventArgs e)
        {
            
                this.sPort.PortName = this.serialPort_cmb.Text;
                this.sPort.BaudRate = 9600;
                this.sPort.DataBits = 8;
                this.sPort.Parity = Parity.None;
                this.sPort.StopBits = StopBits.One;
                this.sPort.Handshake = Handshake.None;
                this.sPort.RtsEnable = true;
                HandleClient();
                label1.Text = "Listining for Clients...";
                label1.Visible=true;
            
            

        }
        
        private void buttonClear_Click(object sender, EventArgs e)
        {
            Console.Clear();
            listBox_ConnectedClients.Items.Clear();
            label1.Text = "";
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}