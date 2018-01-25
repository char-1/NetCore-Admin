export default {
    ADMIN_STORAGE({ commit }, user) {
        commit('ADMIN_STORAGE', user)
    },
    ADMIN_LOGOUT({ commit }, user) {
        commit('ADMIN_LOGOUT', user)
    }
};