FROM microsoft/aspnet:vs-1.0.0-beta4
MAINTAINER Dan Fernandez dan.fernandez@microsoft.com

ADD . /app
RUN apt-get install -y jp2a

WORKDIR /app/approot/src/{{ProjectName}}

ENTRYPOINT ["dnx", ".", "run"]
