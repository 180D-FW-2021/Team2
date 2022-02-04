# build react app into static files
rm -r ../public
cd ../../client
npm install
npm run build
cp -R build ../server/public