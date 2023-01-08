#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#####################
#PUPPETEER RECIPE
#####################
RUN apt-get update && apt-get -f install && apt-get -y install wget gnupg2 apt-utils
RUN wget -q -O - https://dl.google.com/linux/linux_signing_key.pub | apt-key add -
RUN echo 'deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main' >> /etc/apt/sources.list
RUN apt-get update \
&& apt-get install -y google-chrome-stable --no-install-recommends --allow-downgrades fonts-ipafont-gothic fonts-wqy-zenhei fonts-thai-tlwg fonts-kacst fonts-freefont-ttf
######################
#END PUPPETEER RECIPE
######################
ENV PUPPETEER_EXECUTABLE_PATH "/usr/bin/google-chrome-stable"
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HtmlToPdfConverter.WEB/HtmlToPdfConverter.WEB.csproj", "HtmlToPdfConverter.WEB/"]
COPY ["HtmlToPdfConverter.BL/HtmlToPdfConverter.BL.csproj", "HtmlToPdfConverter.BL/"]
COPY ["HtmlToPdfConverter.DAL/HtmlToPdfConverter.DAL.csproj", "HtmlToPdfConverter.DAL/"]
COPY ["Infra.EasySaga/Infra.EasySaga.csproj", "Infra.EasySaga/"]
RUN dotnet restore "HtmlToPdfConverter.WEB/HtmlToPdfConverter.WEB.csproj"
COPY . .
WORKDIR "/src/HtmlToPdfConverter.WEB"
RUN dotnet build "HtmlToPdfConverter.WEB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HtmlToPdfConverter.WEB.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HtmlToPdfConverter.WEB.dll"]