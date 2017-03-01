docker build -t tasks-build-image -f Dockerfile.build .
docker create --name tasks-cont tasks-build-image
docker cp tasks-cont:/app/out ./TaskOutput
docker rm tasks-cont
