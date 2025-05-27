const { defineConfig } = require('@vue/cli-service');
const path = require('path');

module.exports = defineConfig({
  transpileDependencies: true,
  outputDir: path.resolve(__dirname, '../wwwroot'),
  publicPath: '/',
  devServer: {
    proxy: {
      'api':{
        target: 'https://localhost:7292',
        secure: false,
        changeOrigin: true
      }
    }
  }
});
