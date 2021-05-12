### Build image
docker build -t oeeservice .

### create and run container
docker run -it --rm -p 5004:80 --env CONTEXT_URL=http://172.17.0.1:1026 --name oeeservice_sample oeeservice
