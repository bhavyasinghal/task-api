FROM microsoft/aspnetcore:1.0.1
COPY TaskOutput /app
WORKDIR /app 
ENTRYPOINT ["dotnet", "app.dll"]
EXPOSE 80
