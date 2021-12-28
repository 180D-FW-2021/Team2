# build react app into static files
cd ../../client
npm install
npm run build
cp -R build ../server/public