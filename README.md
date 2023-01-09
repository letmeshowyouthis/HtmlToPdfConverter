Scalable and stable web service for HTML to PDF conversions.

I didn't spend too much time on the web client (it's a simple HTML page with little JQuery). I mostly focused on the back-end service.

## Technology Stack
- NET 6
- Docker
- Redis
- MinIO
- RazorPages + JQuery
- Puppeteer Sharp

## How to start
1) Have Docker Desktop running.
2) (Optionally) specify MinIO server credentials in appsettings.json "MinIO" section. By default it's set to "play.min.io".

## Architecture
![arhcitecture](https://user-images.githubusercontent.com/122230590/211382169-771d649e-4167-4d34-95ac-f92a66f36a14.jpg)

## ADR
**Why use sagas?**  
Conversion to PDF must be done as transaction and since we make calls to mutliple external independent services, saga pattern allows us to achieve transactional consistency.

**Why MinIO for storing files?**  
By using MinIO we don't have to have a local file storage, we store files in the cloud. MinIO is made and optimized for storing unstructured data (our case). Also it supports Kubernetes and AWS S3 so we can potentially increase application's scalability further more.

**Why Redis for storing sessions in DB?**  
We interact with sessions much and in order to do it the most performant way we use Redis. Redis stores data in-memory so can we access it at the top possible speed.

**Why Hangfire for utilizing background jobs?**  
Because it's a reliable, scalable and simple-to-use solution for job scheduling. And it's free. Hangfire can be easily scaled just by adding a new machine with a Hangfire server instance and that would require no additional configuration. It's persistent since all background jobs are stored in a persistent storage (SQL server in our case; Hangfire provide Redis storage as well but it's a paid one).
