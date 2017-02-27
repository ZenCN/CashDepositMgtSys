// 源文件头信息：
// <copyright file="OperationResultType.cs">
// Copyright(c)2012-2013 GMFCN.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：郭明锋@中国
// 公司网站：http://www.gmfcn.net
// 所属工程：GMF.Component.Tools
// 最后修改：郭明锋
// 最后修改：2013/05/14 23:04
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace Infrastructure.Operation
{
    /// <summary>
    ///     表示业务操作结果的枚举
    /// </summary>
    [Description("业务操作结果的枚举")]
    public enum ResultType
    {
        /// <summary>
        ///     操作成功
        /// </summary>
        [Description("操作成功。")]
        success,

        /// <summary>
        ///     操作取消或操作没引发任何变化
        /// </summary>
        [Description("操作没有引发任何变化，提交取消。")]
        no_changed,

        /// <summary>
        ///     参数错误
        /// </summary>
        [Description("参数错误。")]
        param_error,

        /// <summary>
        ///     指定参数的数据不存在
        /// </summary>
        [Description("查不到任何数据。")]
        query_nothing,

        /// <summary>
        ///     权限不足
        /// </summary>
        [Description("当前用户权限不足，不能继续操作。")]
        purview_lack,

        /// <summary>
        ///     非法操作
        /// </summary>
        [Description("非法操作。")]
        illegal_operation,

        /// <summary>
        ///     警告
        /// </summary>
        [Description("警告")]
        warning,

        /// <summary>
        ///     操作引发错误
        /// </summary>
        [Description("操作引发错误。")]
        error,
    }
}