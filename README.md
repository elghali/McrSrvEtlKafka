# McrSrvEtlKafka

## To Run Application

### Make Sure Docker is Running


1. From src directory, open the solution in Visual Studio "mcrsrv-etl-kafka.sln"
2. Make sure the directories listed under Parser.API\appsettings.json exist on your computer. Of course, these parameters are expected to be overriden once we ship our application whether via ourdocker-compose.override.yml or any other method you are using to deploy your applications (docker-compose, Helm charts, etc…)
3. Right-click on "docker-compose" and click on "Open in Terminal"
4. From the Terminal run the below command:
```
docker-compose -f .\docker-compose.yml up -d
```
- This will start the `zookeeper` and `broker` services which will consist our "kafka cluster" listening to the port 9092.
- Open Docker Desktop and make sure The containers are running.
- There are two other services `parser.api` and `loader.api` which will fail since we have not added yet any volumes there and the default settings under appsettings.json are pointing to local directories on our Windows machine.

5. A sample file was added to the Repo *employees_202204020001.csv* for the sake of this tutorial. Feel free to move it under the location described in your appsettings.json
6. Now that we have the Kafka cluster up and running and our files in place. From Visual Studio, "Debug" menu, click on "Set Startup Projects" and select "Parser.API" and "Loader.API" projects.
7. Click on "Start" and you should start seeing some messages being generated in the logs while the files are being moved and others created under the "LoaderIncoming" location.
8. To Check things from Kafka side, Go to Docker Desktop dashboard, and look for the broker container, and click on "open in terminal" . Run the below command to check if there are new messages produced and consumed from the topic (in our case it's "employees-etl"). You should find a number other than 0 depicting the number of messages.
```
kafka-consumer-groups --bootstrap-server localhost:9092 --group employees-etl --describe
```
