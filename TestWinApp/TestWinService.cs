using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Diagnostics;

using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.Collections.Generic;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;

/// <summary>
/// Класс объекта сервиса на стороне клиента для выполнения команд от WWW сервера
/// </summary>
namespace TestWinApp
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract()]
    public interface IWinService
    {
        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        void RunNotepad();

        /// <summary>
        /// Метод возвращает сумму аргументов
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>x+y</returns>
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        int Sum(int i, int j);
    }

    /// <summary>
    /// 
    /// </summary>
    class TestWinService : IWinService
    {
        public void RunNotepad()
        {
            Console.WriteLine("Run notepad");
            using (Process exeProcess = Process.Start("notepad.exe"))
            {
                exeProcess.WaitForExit();
            }
        }

        public int Sum(int i, int j)
        {
            Console.WriteLine(String.Format("Sum {0} {1}", i, j));
            return i + j;
        }
    }

    /// <summary>
    /// Класс дополняет поведение сервиса для работы с кросс-доменными запросами в соответствие http://www.w3.org/TR/cors/
    /// </summary>
    public class CustomHeaderMessageInspector : IDispatchMessageInspector
    {
        Dictionary<string, string> requiredHeaders;
        public CustomHeaderMessageInspector(Dictionary<string, string> headers)
        {
            requiredHeaders = headers ?? new Dictionary<string, string>();
        }

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            var httpHeader = reply.Properties["httpResponse"] as HttpResponseMessageProperty;
            foreach (var item in requiredHeaders)
            {
                httpHeader.Headers.Add(item.Key, item.Value);
            }
        }
    }

    /// <summary>
    /// Класс дополняет поведение сервиса для работы с кросс-доменными запросами в соответствие http://www.w3.org/TR/cors/
    /// </summary>
    public class EnableCrossOriginResourceSharingBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {

        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            var requiredHeaders = new Dictionary<string, string>();

            requiredHeaders.Add("Access-Control-Allow-Origin", "*");
            requiredHeaders.Add("Access-Control-Request-Method", "POST,GET,PUT,DELETE,OPTIONS");
            requiredHeaders.Add("Access-Control-Allow-Headers", "X-Requested-With,Content-Type");

            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new CustomHeaderMessageInspector(requiredHeaders));
        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }

        public override Type BehaviorType
        {
            get { return typeof(EnableCrossOriginResourceSharingBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new EnableCrossOriginResourceSharingBehavior();
        }
    }
}
