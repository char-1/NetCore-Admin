const ADMIN_STORAGE = 'ADMIN_STORAGE'
const ADMIN_LOGOUT = 'ADMIN_LOGOUT'
export default {
    [ADMIN_STORAGE](state, user) {
        window.localStorage.setItem('adminStorage', JSON.stringify(user))
        state.adminInfo = user;
    },
    [ADMIN_LOGOUT](state, user) {
        window.localStorage.removeItem('adminStorage')
        state.adminInfo = user;
    }
};