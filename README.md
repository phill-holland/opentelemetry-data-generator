<b>OpenTelemetry Data Generator Console Application</b>

This application demonstrates the simple setup and initalisation of an dotnet core based OpenTelemetry console application, and then generates some dummy data Open Telemtery traces, which can be configured to send to your collection of choice, such as Honeycomb and/or Jaeger.

This application has a number of command line arguments that it takes, to enable this use;

arg1
Endpoint to your collector, such as https://api.honeycomb.io:443

arg2 
ApiKey

arg3 
ServiceName (or DataSet name) - can be anything!

arg4
GRCP or HTTP

If you do not put in these values, it'll resort to defaults, however the defaults will not work!

An application such as this is useful for testing quickly network configuration issues whilst deploying OpenTelemetry inside larger production applications.

Also included by default, is an OTEL_DIAGNOSTICS.json file,
which enables the OpenTelemetry packages to generate further logfiles in the working directory, to help diagnose additional Open Telemetry issues

<ul>
<li><b>Ensure project is open within the development container</b></li>
<li><b>Hit F5</b></li>
</ul>

This application demonstrates the following;

<ul>
<li>VS code debugging and breakpoint functionality</li>
<li>Docker development container configuration</li>
</ul>

Requirements (optional);

The VSCode development container plugin is installed;

https://code.visualstudio.com/docs/remote/containers

Docker must also be installed;

https://docs.docker.com/get-docker/

This application, however is configured with linux based containers, and will not work correctly on Windows without modification.

For further information, go to;

https://dotnet.microsoft.com/download

