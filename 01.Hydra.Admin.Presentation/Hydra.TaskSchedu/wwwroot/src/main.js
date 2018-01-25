// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import iView from 'iview'
import _ from 'lodash'
import md5 from 'md5'
import store from './vuex'
import uuid from 'node-uuid'
import moment from "moment"
import '!style-loader!css-loader!less-loader!./theme/index.less'
Vue.config.productionTip = false
Vue.prototype._ = _
Vue.prototype.md5 = md5
Vue.prototype.staticFileUrl = 'http://192.168.0.180:17122'
Vue.prototype.uuid = uuid
Vue.prototype.moment = moment
Vue.use(iView)
router.beforeEach((to, from, next) => {
    if (to.matched.some(m => m.meta.requiresAuth)) {
        if (store.state.adminInfo.id && store.state.adminInfo.lastLoginToken) {
            next();
        } else {
            next({ path: '/login' });
        }
    } else { next(); }
});
new Vue({
    el: '#app',
    router,
    store,
    template: '<App/>',
    components: { App }
})
