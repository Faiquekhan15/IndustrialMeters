using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WcfDinRail
{
    [ServiceContract]
    public interface IService1
    {
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string[] GetData();

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string MeterData();

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetVoltagesofUnits();


        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetCurrent();


        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string[] GetProperties(ulong Serial);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetFDData();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string GetMuteData();

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetBillData(ulong serial);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetIData(ulong serial);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetLP(ulong serial);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GPApp(ulong Serial);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        int ValidateUser(string auth);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] GetDataMeters();
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string powerfactor();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string tedata();


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string year_consumption();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string tedata2(int serial);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string tedata3(int serial);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]

        string tedata4(int serial);
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]

        string currentreading(int serial);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]

        string getpf(int serial);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]


        string getGroups();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string GetDataMetersWD(string username);
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string GetLPApp(ulong serial, int month);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetLPMeter(ulong serial, int month, int year);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetCircles(string parentNode, string username);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string GetDivisions(string parentNode, string username);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string GetSubDivisions(string parentNode, string username);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetBatches(string parentNode);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string SetInfo(
          string referenceNo,
          string batchNo,
          string circle,
          string divi,
          string subdivi,
          string feeder,
          string txCode,
          string consumerName,
          string cnic,
          string contact,
          string address,
          string sodo,
          string email,
          string tariff,
          string secDep,
          string secDepDate,
          string appNo,
          string appDate,
          string sanctLoad,
          string txCap,
          string loadType,
          string installDate,
          string conndate);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetCustomerInfo(string referenceNo);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetFreeMeters();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string assignMeter(string referenceNo, string serial, string username, string password);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string getUMeters(string username);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetEvents(
          string referenceNo,
          string code,
          int syear,
          int smonth,
          int sday,
          int eyear,
          int eday,
          int emonth);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string GetLPCustomer(
          string referenceNo,
          int smonth,
          int syear,
          int sday,
          int eyear,
          int emonth,
          int eday);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetCustomers(string parentNode);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetCustomerIData(
          string referenceNo,
          int smonth,
          int syear,
          int sday,
          int eyear,
          int emonth,
          int eday);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string GetCustomerBData(
          string referenceNo,
          int smonth,
          int syear,
          int sday,
          int eyear,
          int emonth,
          int eday);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string getMutes(string username);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string getallareas();

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string getallusers();

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string getalladivi(string username);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string insertadivi(string username, string division);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string deleteadivi(string username, string division);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string getInventory();

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string setInventory(string serial, string typ);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string getBillnow(string batch, string username);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string GetLPMeter1(ulong serial, int month, int year);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetDeviceIData(string serial, int smonth, int syear);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string getuserbatches(string username);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string getlatlong(string username);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        List<JObject> mainnode(string username);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        List<JObject> child(string username, string parent);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        List<JObject> grandchild(string username, string parent);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<JObject> greatgrandchild(string username, string parent);
        
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<JObject> mainmeters();

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<MeterProperties> metersInCircle(
          string username,
          string token,
          string parent);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        List<MeterProperties> metersInDivision(
          string username,
          string token,
          string parent);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        List<MeterProperties> metersInBatch(
          string username,
          string token,
          string parent);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        List<MeterProperties> metersInSubDivision(
          string username,
          string token,
          string parent);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<EventData> getMeterEvents(string serial);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        List<MeterProperties> meterProperties(string serial);

        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "authorization_service")]
        ApiKey authorization_service();

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        List<BillGen> bill_gen_service(
          string referenceNo,
          int smonth,
          int syear,
          int sday,
          int eyear,
          int emonth,
          int eday);
    }
}
