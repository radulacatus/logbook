# Logbook

Logbook is a logging engine using RabbitMq, Elasticsearch and Kibana.

----------

##### Prerequisites
 
  - .Net Core 1.1
  - Visual Studio 2017 (+Container development tools)
  - Docker for Windows

----------

##### Run it
 
1. Run Elasticsearch & RabbitMq & Kibana containers
```sh
...logbook\src\Erk> docker-compose up -d
```
 
2. Set docker-compose as startup project  
![N|Solid](https://github.com/radulacatus/logbook/blob/master/res/docker-compose-startup.png?raw=true)
 
3. CTRL+F5 :)
 
4. Log stuff.
