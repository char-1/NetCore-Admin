import axios from 'axios'
import md5 from 'md5'
import store from '../vuex'
import { Modal } from 'iview'
import router from '../router'
const abc = '89c3a5b4b09e40517e212aa8bf36e072'
//HTTP POST 请求
export const HttpPost = (_url, _params, _contentType = 'application/x-www-form-urlencoded') => {
    return new Promise((resolve, reject) => {
        axios.post(_url, _params, {
            headers:
                {
                    'Content-Type': _contentType,
                    'Authorization': store.state.adminInfo.lastLoginToken || '',
                    'Authorer': store.state.adminInfo.loginName || ''
                }
        }).then(function (response) {
            let state = typeof (response.data.state) == 'undefined' ? response.data.result.state : response.data.state;
            switch (state) {
                case 'noAuth':
                case 'faildAuth':
                    Modal.error({
                        title: '系统提示',
                        content: response.data.message,
                        onOk: () => {
                            window.localStorage.removeItem('adminStorage')
                            router.push({
                                name: "Login"
                            });
                        }
                    })
                    break;
                default: resolve(response); break;
            }
        }).catch(function (error) {
            console.log(error)
            reject(error)
        })
    })
};
//HTTP GET 请求
export const HttpGet = (_apiUrl, _params) => {
    return new Promise((resolve, reject) => {
        axios.get(_apiUrl, {
            params: _params,
            headers: {
                "Authorization": store.state.adminInfo.lastLoginToken || ''
            }
        }).then(function (response) {
            let state = typeof (response.data.state) == 'undefined' ? response.data.result.state : response.data.state;
            switch (state) {
                case 'noAuth':
                case 'faildAuth':
                    Modal.error({
                        title: '系统提示',
                        content: response.data.message,
                        onOk: () => {
                            window.localStorage.removeItem('adminStorage')
                            router.push({
                                name: "Login"
                            });
                        }
                    })
                    break;
                default: resolve(response); break;
            }
        }).catch(function (error) {
            reject(error)
        })
    })
};
// 序列化FORM数据 表单参数a=1&b=2&c=3&sign=xxx
export const SerializeForm = (params, sign) => {
    if (params instanceof Object) {
        let signArray = [], tempArray = [], emptyArray = []
        for (var i in params) {
            if (params.hasOwnProperty(i)) {
                if (params[i] !== "")
                    tempArray.push(new Array(i, params[i]))
                else
                    emptyArray.push(i + '=' + '')
            }
        }
        let sortSignArray = tempArray.sort(function (x, y) {
            return x[0].localeCompare(y[0])
        })
        for (var j = 0; j < sortSignArray.length; j++) {
            signArray.push(sortSignArray[j][0] + "=" + sortSignArray[j][1])
        }
        let str = signArray.join('&')
        if (sign)
            signArray.push('sign=' + (md5(str + "&key=" + abc)).toUpperCase())
        let newArray = signArray.concat(emptyArray)
        return newArray.join('&')
    }
    else return ''
};
//生成参数校验 sign
export const MakeSign = (params) => {
    if (params instanceof Object) {
        let signArray = [], tempArray = [], emptyArray = []
        for (var i in params) {
            if (params.hasOwnProperty(i)) {
                if (params[i] !== "")
                    tempArray.push(new Array(i, params[i]))
                else
                    emptyArray.push(i + '=' + '')
            }
        }
        let sortSignArray = tempArray.sort(function (x, y) {
            return x[0].localeCompare(y[0])
        })
        sortSignArray.forEach((item, index) => {
            let value = item[1];
            if (typeof (value) == 'boolean') {
                signArray.push(item[0] + "=" + value.toString().toLowerCase())
            }
            else if (typeof (value) == 'object' || typeof (value) == 'array') {
                let ify = JSON.stringify(value);
                signArray.push(item[0] + "=" + ify.toLowerCase())
            }
            else {
                value = value + "";
                signArray.push(item[0] + "=" + value.toLowerCase())
            }
        })
        let str = signArray.join('&')
        // console.log(str);
        let sign = (md5(str + "&key=" + abc)).toUpperCase();
        params.sign = sign;
        return params;
    }
    else return params;
};

//数字格式化金钱格式 100,000,00
export const FormatMoney = (number, format) => {
    format = typeof (format) == 'undefined' ? 1 : format;//0:tofixed(2);1:decimal;
    let str = "";
    if (format == 1) {
        let flg = false;
        number = number || 0;
        if (number < 0) {
            flg = true;
            number = Math.abs(number);
        }
        let result = number.toString();
        while (result.length > 3) {
            str = ',' + result.slice(-3) + str;
            result = result.slice(0, result.length - 3);
        }
        if (result) { str = (flg == true ? "-" : "") + result + str; }
        return str;
    } else {
        let rate = 100;
        return (number / rate).toFixed(2);
    }
};

