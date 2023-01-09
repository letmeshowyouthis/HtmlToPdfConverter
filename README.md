Scalable and stable web service for HTML to PDF conversions.

I didn't spend too much time on the web client (it's a simple HTML page with little JQuery). I mostly focused on the back-end service.

## Technology Stack
- NET 6
- RazorPages + JQuery
- Docker
- Redis
- MinIO
- Puppeteer Sharp

## How to start
1) Have Docker desktop running.
2) (Optionally) specify MinIO server credentials in appsettings.json "MinIO" section. By default it's set to "play.min.io".

## Architecture
- TODO

## ADR
Why use sagas? Conversion to PDF must be done as transaction and since we make calls to mutliple external independent services, saga pattern allows us to achieve transactional consistency.

Why MnIO for storing files? By using MinIO we don't have to have a local data storage (we store files in the cloud). And it's made and optimized for storing unstructured data. Also it supports Kubernetes and AWS S3 so we can potentially increase application's scalability further more.

Why Redis for storing sessions in DB? We interact with sessions much and in order to do it the most performant way we use Redis. Because Redis stores data in-memory we access it at the top possible speed.

Why Hangfire for utilizing background jobs? Because it's a reliable and scalable solution for job scheduling. And it's free. Hangfire can be easily scaled just by adding new machine with Hangfire server instance and that would require no additional configuration. It's persistent since all background jobs are stored in a persistent storage (SQL server in our case; Hangfire provide Redis storage as well but it's a paid one). And it's simple to use.
