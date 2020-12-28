# 可以使用以下方法安装Vue命令行工具
npm install -g @vue/cli

# 可以使用以下命令行添加快速原型创建工具
npm install -g @vue/cli @vue/cli-service-global

# 以下是常用命令
vue serve   -- 使用快速原型创建的服务启动命令 它会按顺序查找当前目录下的 main.js, index.js, App.vue, app.vue 并且以它为入口进行执行

vue serve MyComponent.vue  -- 使用快速原型命令直接执行 MyComponent.vue 这个文件

vue build  -- 使用快速原型命令创建这个应用 具体可以参见 https://cli.vuejs.org/guide/build-targets.html

# 在网上找到一个有用的 网站后台前端框架 https://www.creative-tim.com/product/vue-black-dashboard?affiliate_id=116187# 这个可以作为参考

# https://cn.vuejs.org/v2/guide/computed.html   这里包含如何使用Vue和第三方 Ajax 和通用工具进行Http请求的工具。 axios  lodash
