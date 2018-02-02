// const ADMIN_BASE_URL = "http://43.247.89.19:8090/API/";//管理后台API(正式地址)
// const ADMIN_BASE_URL = "http://192.168.18.202:8090/API/";//管理后台API(内网测试)
const ADMIN_BASE_URL = "http://192.168.0.180:17120/API/";//管理后台API(本地开发)
export const HTTP_URL_API = {
    //Notice
    GET_NOTICE_LIST: ADMIN_BASE_URL + 'Game/GetNotice',//获取通知公告
    SET_NOTICE_INFO: ADMIN_BASE_URL + 'Game/SetNotice',//设置通知公告
    REM_NOTICE_INFO: ADMIN_BASE_URL + 'Game/RemNotice',//删除通知公告
    EAB_NOTICE_INFO: ADMIN_BASE_URL + 'Game/EabNotice',//启用/停用公告
    //Game
    GET_GAME_CONFIG: ADMIN_BASE_URL + 'Game/GetGameConfig',//获取游戏配置
    SET_GAME_CONFIG: ADMIN_BASE_URL + 'Game/SetGameConfig',//设置游戏配置
    //Task
    GET_TASK_LIST: ADMIN_BASE_URL + 'Game/GetTaskConfig',//获取系统任务列表
    SET_TASK_INFO: ADMIN_BASE_URL + 'Game/SetTaskConfig',//配置系统任务
    REMOVE_TASK_INFO: ADMIN_BASE_URL + 'Game/RemTaskConfig',//删除任务
    ENABLE_TASK_INFO: ADMIN_BASE_URL + 'Game/EabTaskConfig',//启用任务 
    SINGLE_TASK_INFO: ADMIN_BASE_URL + 'Game/SglTaskConfig',//获取任务(单条)
    //BUG
    SET_BUG_LIST: ADMIN_BASE_URL + 'Game/SetBug',//采纳意见建议
    REM_BUG_INFO: ADMIN_BASE_URL + 'Game/RemBug',//删除意见建议
    GET_BUG_LIST: ADMIN_BASE_URL + 'Game/GetBugs',//意见建议列表
    //Email
    GET_EMAIL_LIST: ADMIN_BASE_URL + 'Game/GetMails',//获取邮件列表
    SEND_MAIL_INFO: ADMIN_BASE_URL + 'Game/SendMail',//发送站内邮件
    //Single Data
    GET_SINGLE_CONFIG: ADMIN_BASE_URL + 'Game/GetSingleConfig',//获取大转盘配置
    SET_SINGLE_CONFIG: ADMIN_BASE_URL + 'Game/SetSingleConfig',//设置大转盘配置
    ADD_ROBOT_INFO: ADMIN_BASE_URL + 'Game/CreateRobot',//手动创建机器人 
    //Shop
    ADD_SHOP_PRODUCT: ADMIN_BASE_URL + 'Game/SetProduct',//商城商品新增
    DEL_SHOP_PRODUCT: ADMIN_BASE_URL + 'Game/RemProduct',//删除商城商品
    GET_SHOP_PRODUCT: ADMIN_BASE_URL + 'Game/GetProduct',//获取商城商品
    //Player
    GET_PLAYER_LIST: ADMIN_BASE_URL + 'Game/GetPlayers',//玩家列表
    GET_PLAYER_INFO: ADMIN_BASE_URL + 'Game/GetPlayer',//获取玩家信息
    PLAYER_ADD_GOLD: ADMIN_BASE_URL + 'Game/SetPlayerGold',//玩家上下分/VIP等级修改
    SET_PLAYER_FREEZE: ADMIN_BASE_URL + 'Game/SetPlayerFreeze',//解/锁玩家 
    GET_PLAYER_RELATED: ADMIN_BASE_URL + 'Game/GetPlayerRelated',//获取玩家相关信息
    GET_PLAYER_GOLDRANK: ADMIN_BASE_URL + 'Game/GetPlayerGoldRank',//获取玩家金币排行
    GET_PLAYER_DIALRECORD: ADMIN_BASE_URL + 'Game/GetDialRecord',//获取玩家抽奖记录
    //Spark
    GET_SPARK_CONFIG: ADMIN_BASE_URL + 'Game/GetSpark',//获取spark配置
    SET_SPARK_CONFIG: ADMIN_BASE_URL + 'Game/SetSpark',//设置spark配置
    //CashRecord
    GET_PLAYER_ORDER: ADMIN_BASE_URL + 'Game/GetPlayerOrder',//获取玩家订单(充值/提现)
    SET_PLAYER_CASHRECORD: ADMIN_BASE_URL + 'Game/HandleCashRecord',//处理提现申请
    //ADMIN
    ADMIN_LOGIN: ADMIN_BASE_URL + 'Admin/Login',//管理员登入
    ADMIN_LOGOUT: ADMIN_BASE_URL + 'Admin/Logout',//管理员登出
    GET_ADMIN_LIST: ADMIN_BASE_URL + 'Admin/Grid',//管理员列表
    ADD_ADMIN_INFO: ADMIN_BASE_URL + 'Admin/Add',//管理员新增
    UPDATE_ADMIN_INFO: ADMIN_BASE_URL + 'Admin/Update',//管理员编辑
    DELETE_ADMIN_INFO: ADMIN_BASE_URL + 'Admin/Remove',//管理员删除
    SET_ADMIN_ENABLE: ADMIN_BASE_URL + 'Admin/Enable',//设置启用
    GET_ADMIN_SINGLE: ADMIN_BASE_URL + 'Admin/Find',//获取单条管理员
    UPDATE_ADMIN_PASS: ADMIN_BASE_URL + 'Admin/ModifyPass',//修改管理员登录密码
    //ROLE
    GET_ROLE_LIST: ADMIN_BASE_URL + 'Role/Grid',//角色列表
    ADD_ROLE_INFO: ADMIN_BASE_URL + 'Role/Add',//角色新增
    UPDATE_ROLE_INFO: ADMIN_BASE_URL + 'Role/Update',//角色修改
    DELETE_ROLE_INFO: ADMIN_BASE_URL + 'Role/Remove',//角色删除
    SET_ROLE_ENABLE: ADMIN_BASE_URL + 'Role/Enable',//设置启用
    GET_ROLE_PERMISSION: ADMIN_BASE_URL + 'Role/Permission',//获取角色权限
    GET_ROLE_SINGLE: ADMIN_BASE_URL + 'Role/Find',//获取单条角色 
    GET_ROLE_SELECT: ADMIN_BASE_URL + 'Role/Select',//获取角色下拉集合
    SET_ROLE_PERMISSION: ADMIN_BASE_URL + 'Role/SetPerission',//设置角色权限        
    //MENU
    GET_MENU_TREE: ADMIN_BASE_URL + 'System/MenuTree',//获取权限列表 TREE
    GET_MENU_CASCADER: ADMIN_BASE_URL + 'System/MenuCascader',//获取权限列表 CASCADER
    ADD_MENU_INFO: ADMIN_BASE_URL + 'Menu/Add',//新增菜单
    GET_MENU_SINGLE: ADMIN_BASE_URL + 'Menu/Find',//获取单条菜单
    UPDATE_MENU_INFO: ADMIN_BASE_URL + 'Menu/Update',//修改菜单
    DELETE_MENU_INFO: ADMIN_BASE_URL + 'Menu/Remove',//删除菜单
    //SYSTEM
    SET_SYSTEM_CONFIG: ADMIN_BASE_URL + 'System/SetConfig',//设置系统配置参数
    GET_SYSTEM_CONFIG: ADMIN_BASE_URL + 'System/GetConfig',//获取系统配置参数    
    FILE_SYSTEM_UPLOAD: ADMIN_BASE_URL + 'System/Upload',//配置系统参数
    //数据概况
    GET_DASHBOARD_CHART: ADMIN_BASE_URL + 'Analysis/DashBoard',//获取DashBoard面板统计数据
    GET_DASHBOARD_GRID: ADMIN_BASE_URL + 'Analysis/DashBoardGrid',//获取DashBoardGrid面板统计数据
    GET_REMAIN_GRID: ADMIN_BASE_URL + 'Analysis/RemainGrid',//获取RemainGrid面板统计数据
    GET_PLAYER_GOLD: ADMIN_BASE_URL + 'Analysis/PlayerGold',//获取玩家金币流水
    GET_PLAYER_ONLINE: ADMIN_BASE_URL + 'Analysis/PlayerOnline',//获取玩家在线走势图 
    GET_GAME_PROFIT: ADMIN_BASE_URL + 'Analysis/GameProfit',//获取平台盈利统计
    GET_PROFIT_DETAIL: ADMIN_BASE_URL + 'Analysis/ProfitDetail',//获取平台盈利明细
    GET_LOGS_LIST: ADMIN_BASE_URL + 'System/Logs',//获取系统操作日志
    DELETE_LOGS_INFO: ADMIN_BASE_URL + 'System/RmLogs',//删除系统操作日志    
};