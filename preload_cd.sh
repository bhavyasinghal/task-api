docker build \
    -f Dockerfile.unittest \
    -t 10.100.198.200:5000/taskapi-unittests \
    .

docker push 10.100.198.200:5000/taskapi-unittests 

docker build \
    -f Dockerfile.test \
    -t 10.100.198.200:5000/taskapi-tests \
    .

docker push 10.100.198.200:5000/taskapi-tests

docker-compose \
    -f docker-compose-dev.yml \
    run --rm tests

docker build \
    -f Dockerfile \
    -t 10.100.198.200:5000/taskapi \
    .

docker push 10.100.198.200:5000/taskapi
