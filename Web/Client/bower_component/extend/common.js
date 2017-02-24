//----------------------window--------------------
window.isString = function(str) {
    return typeof str == 'string' && str.trim().length > 0;
};

//----------------------date----------------------
Date.prototype.get_day = function(d) { //default today(undefined)、oneday: -7 7、lastday 0
    var date = new Date(); //default today

    if (d != undefined) {
        if (typeof d == 'number') {
            if (d != 0) { //oneday
                date = date.valueOf();
                date = date + d * 24 * 60 * 60 * 1000;
                date = new Date(date);
            } else { //this month lastday
                date = new Date(date.getFullYear(), date.getMonth(), 0);
            }
        }
    }

    return date;
};

Date.prototype.get_lastday = function(m) {
    var y = new Date().getFullYear(), date;

    if (angular.isArray(m)) {
        date = [];
        angular.forEach(m, function(val) {
            val = parseInt(val) < 10 ? ('0' + val) : val;
            date.push(val + '月' + new Date(y, val, 0).getDate() + '日')
        });
    } else {
        m = parseInt(m) < 10 ? ('0' + m) : m;
        date = m + '月' + new Date(y, m, 0).getDate() + '日';
    }

    return date;
};

Date.prototype.to_str = function(formate) { //是Date对象才可以调用to_str方法
    var m = this.getMonth() + 1;
    if (m < 10) {
        m = '0' + m;
    }

    var d = this.getDate();
    if (d < 10) {
        d = '0' + d;
    }

    switch (formate) {
    case '-':
        return this.getFullYear() + "-" + m + "-" + d;
    case 'MM月dd日':
        return m + "月" + d + '日';
    default:
        return this.getFullYear() + "年" + m + "月" + d + '日';
    }

};

Date.convert = function(val) {
    if (typeof val != "string" || val.trim().length <= 0) {
        return undefined;
    }
    var dt = new Date(parseInt(val.toLowerCase().replace("/date(", "").replace(")/", "").split("+")[0]));
    var m = dt.getMonth() + 1;
    if (m < 10) {
        m = '0' + m;
    }

    var d = dt.getDate();
    if (d < 10) {
        d = '0' + d;
    }

    return dt.getFullYear() + "-" + m + "-" + d;
};

//---------------------string---------------------
String.prototype.replace_all = function(s1, s2) {
    return this.replace(new RegExp(s1, "gm"), s2); //g全局     
};

String.prototype.contains = function(str) {
    if (typeof(str) == "string" && this.indexOf(str) >= 0) {
        return true;
    } else {
        return false;
    }
};

//---------------------array----------------------
Array.prototype.extract = function(keys) {
    var arr = [];
    if (keys.length) {
        var obj;
        for (var i = 0; i < this.length; i++) {
            obj = {};
            for (var j = 0; j < keys.length; j++) {
                if (this[i][keys[j]] == null || this[i][keys[j]] == 0 ||
                    typeof this[i][keys[j]] == "string" && this[i][keys[j]].trim().length == 0 ||
                    typeof this[i][keys[j]] == "number" && this[i][keys[j]] == 0) {
                    continue;
                }
                obj[keys[j]] = this[i][keys[j]];
            }

            if (!$.isEmptyObject(obj)) {
                arr.push(obj);
            }
        }
    }

    return arr;
};

Array.prototype.the_first = function() { //注意：不要跟jquery的first方法冲突
    if (this.length) {
        return this[0];
    } else {
        return undefined;
    }
};

Array.prototype.the_last = function() {
    if (this.length) {
        return this[this.length - 1];
    } else {
        return undefined;
    }
};

Array.prototype.seek = function(predicate, match_val, option) {
    if (this == null) {
        throw new TypeError('Array.prototype.find called on null or undefined');
    }

    var list = Object(this);
    var length = list.length >>> 0;
    var value;

    if (typeof predicate == 'function') {
        var thisArg = arguments[1];
        for (var i = 0; i < length; i++) {
            value = list[i];
            //while thisArg is function 'return false' in func inner will break the func
            if (predicate.call(thisArg, value, i, list)) {
                return value;
            }
        }
    } else if (typeof predicate == "string") {
        for (var i = 0; i < length; i++) {
            value = list[i];
            if (value[predicate] == match_val) {
                switch (typeof option) {
                case 'string':
                    if (option == 'del') {
                        list.splice(i, 1);
                        return true;
                    } else {
                        return value[option || predicate]; //return the appointed key
                    }
                default:
                    return value; //找到后默认返回obj
                }
            }
        }
    }

    return false; //没有找到返回false
};

Array.prototype.exist = function(e) { //e可为string、number类型或等于null、undefined
    var exist = false;
    if (typeof e == "string") {
        var s = String.fromCharCode(2);
        exist = new RegExp(s + e + s).test(s + this.join(s) + s);
    } else {
        for (var i = 0; i < this.length; i++) {
            if (this[i] == e) {
                exist = true;
                break;
            }
        }
    }

    return exist;
};

//---------------------number----------------------
Number.prototype.toFixed = function(exponent) { //overwrite toFixed function
    if (exponent) {
        var result = (parseInt(this * Math.pow(10, exponent) + 0.5) / Math.pow(10, exponent)).toString();
        var count = 0;
        if (result.indexOf(".") > 0) {
            count = exponent - result.split(".")[1].length;
        } else {
            count = exponent;
            result += ".";
        }

        for (count; count > 0; count--) {
            result += "0";
        }

        return result;
    } else {
        return parseInt(this);
    }
};

//---------------------number----------------------
window.calc = {
    addition: function(arg1, arg2) {
        var r1,
            r2,
            m,
            n,
            result;
        arg1 = arg1 == undefined ? 0 : arg1;
        arg2 = arg2 == undefined ? 0 : arg2;
        try {
            r1 = arg1.toString().split(".")[1].length;
        } catch (e) {
            r1 = 0;
        }
        try {
            r2 = arg2.toString().split(".")[1].length;
        } catch (e) {
            r2 = 0;
        }
        m = Math.pow(10, Math.max(r1, r2));
        n = (r1 >= r2) ? r1 : r2;
        result = ((arg1 * m + arg2 * m) / m).toFixed(n);
        return Number(result) <= 0 ? undefined : result;
    },
    subtraction: function(arg1, arg2) {
        var r1,
            r2,
            m,
            n,
            result;
        arg1 = arg1 == undefined ? 0 : arg1;
        arg2 = arg2 == undefined ? 0 : arg2;
        try {
            r1 = arg1.toString().split(".")[1].length;
        } catch (e) {
            r1 = 0;
        }
        try {
            r2 = arg2.toString().split(".")[1].length;
        } catch (e) {
            r2 = 0;
        }
        m = Math.pow(10, Math.max(r1, r2));
        n = (r1 >= r2) ? r1 : r2;
        result = ((arg1 * m - arg2 * m) / m).toFixed(n);
        return Number(result) <= 0 ? undefined : result;
    },
    multiplication: function(arg1, arg2) //乘法
    {
        var m = 0,
            s1 = arg1.toString(),
            s2 = arg2.toString();
        try {
            m += s1.split(".")[1].length
        } catch (e) {
        }
        try {
            m += s2.split(".")[1].length
        } catch (e) {
        }
        return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m);
    },
    division: function(arg1, arg2, n) //除法
    {
        arg1 = arg1 == undefined ? 0 : arg1;
        arg2 = arg2 == undefined ? 0 : arg2;
        if (arg1 == 0 || arg2 == 0) {
            return 0; //此处不能返回undefined
        } else {
            var t1 = 0,
                t2 = 0,
                r1,
                r2;
            try {
                t1 = arg1.toString().split(".")[1].length;
            } catch (e) {
            }
            try {
                t2 = arg2.toString().split(".")[1].length;
            } catch (e) {
            }
            with (Math) {
                r1 = Number(arg1.toString().replace(".", ""));
                r2 = Number(arg2.toString().replace(".", ""));
                n = n == undefined ? 4 : n;
                return parseFloat(((r1 / r2) * pow(10, t2 - t1)).toFixed(n));
            }
        }
    }
}