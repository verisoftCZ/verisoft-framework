# Build & Deployment
## Build
Build probíhá v Azure Devops a to pomocí automatických build pipelines
Každá servisa má CI build pro validaci a Docker build pro vytvoření kontejneru, který se následně nahraje do docker registry odkud si ho může následně stáhnout cluster dle nastavení deployment.yaml (viz níže)

CI build (Verisoft.DemoApi.CI.yml)
Součástí CI buildu musí být spuštění TEST projektů
Součástí CI buildů musí být kontrola na Warnings a musí být brány jako chyby
``` yaml
trigger:
  branches:
    include:
    - master
    - releases/*
  paths:
    include:
    - src/Verisoft.DemoApi

pool:
  vmImage: 'ubuntu-latest'
  name: 'VerisoftHosted'

variables:
  solution: 'src/Verisoft.DemoApi.sln'
  buildConfiguration: 'Release'
  netCoreSdkVersion: '8.x'

steps:
- task: UseDotNet@2
  displayName: 'Install .NET Core SDK $(netCoreSdkVersion)'
  inputs:
    version: $(netCoreSdkVersion)

- task: NuGetToolInstaller@1

- task: DotNetCoreCLI@2
  inputs:
    command: 'restore'
    projects: '$(solution)'
    feedsToUse: 'select'
    vstsFeed: '5e1c1cd0-6e9d-43e0-a570-faca2b5ac27c'

- task: DotNetCoreCLI@2
  inputs:
    command: 'build'
    projects: '$(solution)'
    arguments: '-c $(buildConfiguration) /warnaserror'
    
- task: DotNetCoreCLI@2
  inputs:
    command: 'test'
    projects: '$(solution)'
```


Docker build (Verisoft.DemoApi.Docker.yml)
``` yaml
trigger:
  branches:
    include:
    - master
    - releases/*
  paths:
    include:
    - src/Verisoft.DemoApi

pool:
  vmImage: 'ubuntu-latest'
  name: 'VerisoftHosted'

variables:
  tag: '$(Build.BuildId)'
  imageName: 'verisoft/fmw/demoapi'
  backendDirectory: 'src/Verisoft.DemoApi/Verisoft.DemoApi.Host'

steps:
- task: Docker@2
  inputs:
    containerRegistry: 'verisoft'
    repository: $(imageName)
    command: 'buildAndPush'
    Dockerfile: '$(backendDirectory)/Dockerfile'
    buildContext: 'src'
    tags: |
      $(Build.BuildId)
      latest
```


## Deployment
Release probíha v Azure Devops a to pomocí automatických release pipelines
Pro každou mikroservisu, kterou chceme nasadit na cluster je nutné vytvořit deploy.yaml a service.yaml
Deploy:
Slouží pro nasazení na cluster
**!!Pro každou mikroservisu musí být nastaveny resources!!** Aby nedošlo k vyčerpání zdrojů clusterů a nějaká služba neměla dostupné neomezené zdroje.
``` yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: verisoft-fmw-demoapi
  namespace: dev
spec:
  replicas: 1
  revisionHistoryLimit: 2
  selector: 
    matchLabels:
      app: verisoft-fmw-demoapi
  template:
    metadata:
      labels:
        app: verisoft-fmw-demoapi
    spec:
      containers:
      - name: verisoft-fmw-demoapi
        image: verisoft.azurecr.io/verisoft/fmw/demoapi:1  ##ZDE nutné změnit dle container registry
        imagePullPolicy: Always
        ports:
        - containerPort: 80
        env:
        - name: "ASPNETCORE_ENVIRONMENT"
          value: "DEV"
        resources:
          requests:
            memory: "200Mi"
            cpu: "100m"
          limits:
            memory: "300Mi"
```

Service:
Slouží pro registraci Service (zjednodušeně routing)
``` yaml
apiVersion: v1
kind: Service
metadata:
  name: verisoft-fmw-demoapi
  namespace: dev
spec:
  selector:
    app: verisoft-fmw-demoapi
  ports:
  - protocol: TCP
    port: 80
    targetPort: 80
```

Container registry
Container registry je vytvořen v Azure jako služba  a nahráváme do něj containers pomocí build pipelines.
Cluster pak používá tento container registry aby si jednotlivé container stáhnul dle čísla buildu, případně pomocí tagu latest.
Číslo buildu se do release pipeline předává automaticky pomocí artefaktu.