using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace DiagnosticSourceSample
{
    class Program
    {

        static async Task Main(string[] args)
        {
            Subcribe();
            var number = new MySampleLibrary().GetRandomNumber();
           await MySampleLibrary.DoThingAsync(number);
            var httpClient = new HttpClient();
            await httpClient.GetAsync("https://kalapos.net");
            Console.WriteLine("Hello World!");
        }
        private static void Subcribe()
        {
            DiagnosticListener.AllListeners.Subscribe(new Subcriber());
        }
    }
    public class MyLibraryListener : IObserver<KeyValuePair<string, object>>
    {
        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(KeyValuePair<string, object> keyValue)
        {
            switch (keyValue.Key)
            {
                case "DoThingAsync.Start":
                    Console.WriteLine($"DoThingAsync.Start - activity id:{Activity.Current?.Id}");

                    break;
                case "DoThingAsync.Stop":
                    Console.WriteLine("DoThingAsync.Stop");
                    if(Activity.Current!=null)
                    {
                        foreach(var tag in Activity.Current.Tags)
                        {
                            Console.WriteLine($"{tag.Key} - {tag.Value}");
                        }
                    }
                    break;
                case "DiagnosticSourceSample.MySampleLibrary.StartGenerateRandom":
                    Console.WriteLine("Start generate random");
                    break;
                case "DiagnosticSourceSample.MySampleLibrary.EndGenerateRandom":
                    var randomValue = keyValue.Value.GetType().GetTypeInfo().GetDeclaredProperty("RandomNumber").GetValue(keyValue.Value);
                    Console.WriteLine($"StopGenerateRandom Generated random value: {randomValue}");
                    break;
                default:
                    break;
            }
        }
    }

    public class HttpClientObserver : IObserver<KeyValuePair<string, object>>
    {
        public static Stopwatch _stopwatch = new Stopwatch();
        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(KeyValuePair<string, object> recievedEvent)
        {
            switch (recievedEvent.Key)
            {
                case "System.Net.Http.HttpRequestOut.Start":
                    _stopwatch.Start();

                    if (recievedEvent.Value.GetType().GetTypeInfo().GetDeclaredProperty("Request")?.GetValue(recievedEvent.Value) is HttpRequestMessage requestMessage)
                    {
                        Console.WriteLine($"HTTP Request start:{requestMessage.Method} - {requestMessage.RequestUri}");
                    }
                    break;
                case "System.Net.Http.HttpRequestOut.Stop":
                    _stopwatch.Stop();
                    if (recievedEvent.Value.GetType().GetTypeInfo().GetDeclaredProperty("Response")?.GetValue(recievedEvent.Value) is HttpResponseMessage responseMessage)
                    {
                        Console.WriteLine($"HTTP Request finish: took {_stopwatch.ElapsedMilliseconds}ms, status code: {responseMessage.StatusCode} - parent Activity Id: {Activity.Current.ParentId}");
                    }
                    break;
                default:
                    break;
            }
        }
    }
    public class Subcriber : IObserver<DiagnosticListener>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(DiagnosticListener listener)
        {
            if (listener.Name == typeof(MySampleLibrary).FullName)
            {
                listener.Subscribe(new MyLibraryListener());
            }
            if (listener.Name == "HttpHandlerDiagnosticListener")
            {
                listener.Subscribe(new HttpClientObserver());
            }
        }
    }
    public class MySampleLibrary
    {
        private static readonly DiagnosticSource diagnosticSource =
            new DiagnosticListener(typeof(MySampleLibrary).FullName);
        public int GetRandomNumber()
        {
            if (diagnosticSource.IsEnabled(typeof(MySampleLibrary).FullName))
            {
                diagnosticSource.Write($"{typeof(MySampleLibrary).FullName}.StartGenerateRandom", null);
            }

            var random = new Random().Next(100, 1000);
            if (diagnosticSource.IsEnabled(typeof(MySampleLibrary).FullName))
            {
                diagnosticSource.Write($"{typeof(MySampleLibrary).FullName}.EndGenerateRandom",
                    new { RandomNumber = random });
            }
            return random;
        }

        public static async Task DoThingAsync(int id)
        {
            var activity = new Activity(nameof(DoThingAsync));
            if (diagnosticSource.IsEnabled(typeof(MySampleLibrary).FullName))
            {
                diagnosticSource.StartActivity(activity, new { IdArg = id });
            }
            activity.AddTag("MyTabId","ValueInTags");
            activity.AddBaggage("MyBaggageId", "ValueInBaggage");
            var httpClient = new HttpClient();
            await httpClient.GetAsync("http://localhost:59500/weatherforecast");
            if (diagnosticSource.IsEnabled(typeof(MySampleLibrary).FullName))
            {
                diagnosticSource.StopActivity(activity, new { IdArg = id });
            }
        }
    }
}
