import Vue from 'vue'
import Vuex from 'vuex'
import mutations from './mutations'
import actions from './actions'
Vue.use(Vuex)
const state = {
    adminInfo:JSON.parse(window.localStorage.getItem('adminStorage')) || {}
}
export default new Vuex.Store({
    state,
    actions,
    mutations
})
