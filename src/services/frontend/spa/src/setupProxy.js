const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function(app) {
    app.use(
        '/api/catalog',
        createProxyMiddleware({
            target: process.env.REACT_APP_CATALOG_HREF,
            changeOrigin: true,
        })
    );

    app.use(
        '/identity',
        createProxyMiddleware({
            target: process.env.REACT_APP_SSO_HREF,
            changeOrigin: true,
        })
    );
};