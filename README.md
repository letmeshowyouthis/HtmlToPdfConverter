Scalable and stable web service for HTML to PDF conversions.

I didn't spend too much time on the web client (it's a simple HTML page with little JQuery). I mostly focused on the back-end service.

## Technology Stack
- NET 6
- RazorPages + JQuery
- Puppeteer Sharp
- Redis
- MinIO

## How to start
1) Have a running redis server.
2) Specify "RedisConnectionString" in appsettings.json.
3) Have a running SQL server with a database created.
4) Specify "Hangfire.ConnectionString" in appsetting.json.
5) (Optionally) specify MinIO server credentials in "MinIO" section. By default it's set to "play.min.io".

## Architecture
- TODO

## ADR
Why use sagas? Conversion to PDF must be done as part of one transaction and since during conversion we make calls to mutliple external independent services, saga pattern helps us to achieve transactional consistency.

Why MnIO for storing files? Because it's made and optimized for storing objects. By using MinIO we don't have to have a local data storage (we store files in a cloud storage). Also it supports Kubernetes and AWS S3 so we can potentially increase application's scalability even more.

Why Redis for storing sessions in DB? Because since we interact with sessions much we do it in the most performant way by using Redis. Redis stores data in-memory so we can access it at the top possible speed.

Why Hangfire for utilizing background jobs? Because it's a reliable and scalable solution and it's free. It's computing power can be easily scaled just by adding new machine with Hangfire and that would require no additional configuration. It's persistent since all background jobs are stored in a persistent storage (SQL server in our case; Hangfire provide Redis storage as well but it's a paid one). And it's simple to use.
