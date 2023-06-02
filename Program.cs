using System.Diagnostics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Net;
using System.Runtime.ExceptionServices;

static void FirstChanceExceptionHandler(object sender, FirstChanceExceptionEventArgs args)
{
    var activity = Activity.Current;

    while(activity != null)
    {
        activity.RecordException(args.Exception);
        activity.SetStatus(Status.Error);
        activity.Dispose();
        activity = activity.Parent;
    }
}

string endPoint = "https://api.honeycomb.io:443";
string apiKey = "MyApiKey";
string serviceName = "MyServiceName";

OpenTelemetry.Exporter.OtlpExportProtocol protocol = 
    OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;

Console.WriteLine("Usage: opentelemetry-data-generator endpoint apikey servicename mode");

if(args.Length > 0) endPoint = args[0];
if(args.Length > 1) apiKey = args[1];
if(args.Length > 2) serviceName = args[2];
if(args.Length > 3)
{
    if(args[3].CompareTo("HTTP") == 0)
    {
        protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
        string[] values = endPoint.Split('/');
        if(values.Length < 5) endPoint = endPoint + "/v1/traces";
        else 
        {
            if((values[values.Length -2] != "v1")&&(values[values.Length - 1]!= "traces"))
            {
                endPoint = endPoint + "/v1/traces";
            }
        }
    }
}

Console.WriteLine($"opentelemetry-data-generator {endPoint} {apiKey} {serviceName}");

ActivitySource activitySource = new ActivitySource(serviceName);

using var tracerProvider = OpenTelemetry.Sdk.CreateTracerProviderBuilder()
                .AddSource(serviceName)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()   
                .AddConsoleExporter()                
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName)) 
                .AddOtlpExporter(option =>
                {
                    option.Endpoint = new Uri(endPoint);
                    option.Headers = $"X-Honeycomb-Team={apiKey}";
                    option.Protocol = protocol;
                })      
                .SetErrorStatusOnException()      
                .Build();

Tracer tracer = tracerProvider.GetTracer(serviceName);

const int iterations = 10;

string []words = { "banana", "giraffe", "cheese", "monkey", "cat", "badger", "apple" };
for(int i = 0; i < iterations; ++i)
{
    HttpClient client = new HttpClient();    
    if(i % 10 == 0)
    {
        // intentionally generator a 404 page not found error
        await client.GetAsync("http://www.google.co.uk/seaffh?q=vanilla");
    }
    else
    {
        // generate a search from google
        await client.GetAsync("http://www.google.co.uk/search?q=" + words[i % words.Length]);
    }
}

AppDomain.CurrentDomain.FirstChanceException += FirstChanceExceptionHandler;

throw new Exception("oh dear! oh no!");
