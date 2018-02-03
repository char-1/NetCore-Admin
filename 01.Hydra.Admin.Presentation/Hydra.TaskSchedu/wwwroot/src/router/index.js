import Vue from 'vue'
import Router from 'vue-router'
import Home from '@/pages/Home'
import Dashboard from '@/pages/Dashboard'
import Tactful from '@/pages/Game/Tactful'
import ConfigTab from '@/pages/System/Config'
import OperateLog from '@/pages/System/OperateLog'
import Shop from '@/pages/Game/Shop'
import Exchange from '@/pages/Game/Exchange'
import Menu from '@/pages/System/Menu'
import Admin from '@/pages/System/Admin'
import Role from '@/pages/System/Role'
import Email from '@/pages/Game/Email'
import Proposal from '@/pages/Game/Proposal'
import Turntable from '@/pages/Game/Turntable'
import Task from '@/pages/Game/Task'
import BaseSet from '@/pages/Game/BaseSet'
import Spark from '@/pages/Game/Spark'
import Robot from '@/pages/Game/Robot'
import Notice from '@/pages/Game/Notice'
import User from '@/pages/User/Index'
import AllUser from '@/pages/User/All'
import AddUser from '@/pages/User/Add'
import OnlineUser from '@/pages/User/Online'
import AgainUser from '@/pages/User/Again'
import RechargeUser from '@/pages/User/Recharge'
import CashRecord from '@/pages/User/CashRecord'
import DialRecord from '@/pages/User/DialRecord'
import GamePayment from '@/pages/Analysis/GamePayment'
import RechargePayment from '@/pages/Analysis/RechargePayment'
import PlatformPayment from '@/pages/Analysis/PlatformPayment'
import LeveleDistribute from '@/pages/Analysis/LeveleDistribute'
import Login from '@/pages/Login'
import GameTab from '@/pages/Game/Setting'
import KeepPlayer from '@/pages/Analysis/KeepPlayer'
import GoldRank from '@/pages/Analysis/GoldRank'
import OnlineState from '@/pages/Analysis/OnlineState'
import GoldTable from '@/pages/Analysis/GoldTable'
Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      component: Home,
      children: [
        {
          path: '/',
          name: 'Dashboard',
          component: Dashboard, meta: { requiresAuth: true }
        },
        {
          path: 'proposal-table',
          name: 'Proposal',
          component: Proposal, meta: { requiresAuth: true }
        },
        {
          path: 'turntable-table',
          name: 'Turntable',
          component: Turntable, meta: { requiresAuth: true }
        },
        {
          path: 'config-tab',
          name: 'ConfigTab',
          component: ConfigTab, meta: { requiresAuth: true }
        },
        {
          path: 'operate-log-table',
          name: 'OperateLog',
          component: OperateLog, meta: { requiresAuth: true }
        },
        {
          path: 'task-table',
          name: 'Task',
          component: Task, meta: { requiresAuth: true }
        },
        {
          path: 'notice-table',
          name: 'Notice',
          component: Notice, meta: { requiresAuth: true }
        },        
        {
          path: 'base-setting',
          name: 'BaseSet',
          component: BaseSet, meta: { requiresAuth: true }
        },        
        {
          path: 'tactful-table',
          name: 'Tactful',
          component: Tactful, meta: { requiresAuth: true }
        },
        {
          path: 'email-table',
          name: 'Email',
          component: Email, meta: { requiresAuth: true }
        },
        {
          path: 'spark-table',
          name: 'Spark',
          component: Spark, meta: { requiresAuth: true }
        },       
        {
          path: 'robot-config',
          name: 'Robot',
          component: Robot, meta: { requiresAuth: true }
        },           
        {
          path: 'shop-table',
          name: 'Shop',
          component: Shop, meta: { requiresAuth: true }
        },
        {
          path: 'exchange-table',
          name: 'Exchange',
          component: Exchange, meta: { requiresAuth: true }
        },        
        {
          path: 'menu-tree',
          name: 'Menu',
          component: Menu, meta: { requiresAuth: true }
        },
        {
          path: 'role-table',
          name: 'Role',
          component: Role, meta: { requiresAuth: true }
        },
        {
          path: 'admin-table',
          name: 'Admin',
          component: Admin, meta: { requiresAuth: true }
        },
        {
          path: 'alluser-table',
          name: 'AllUser',
          component: AllUser, meta: { requiresAuth: true }
        },
        {
          path: 'add-table',
          name: 'AddUser',
          component: AddUser, meta: { requiresAuth: true }
        },
        {
          path: 'online-table',
          name: 'OnlineUser',
          component: OnlineUser, meta: { requiresAuth: true }
        },       
        {
          path: 'again-table',
          name: 'AgainUser',
          component: AgainUser, meta: { requiresAuth: true }
        },
        {
          path: 'credit-table',
          name: 'RechargeUser',
          component: RechargeUser, meta: { requiresAuth: true }
        },        
        {
          path: 'cash-record',
          name: 'CashRecord',
          component: CashRecord, meta: { requiresAuth: true }
        },        
        {
          path: 'dial-record',
          name: 'DialRecord',
          component: DialRecord, meta: { requiresAuth: true }
        },                    
        {
          path: 'game-tab',
          name: 'GameTab',
          component: GameTab, meta: { requiresAuth: true }
        },
        {
          path: 'game-payment',
          name: 'GamePayment',
          component: GamePayment, meta: { requiresAuth: true }
        },
        {
          path: 'recharge-payment',
          name: 'RechargePayment',
          component: RechargePayment, meta: { requiresAuth: true }
        },
        {
          path: 'platform-payment',
          name: 'PlatformPayment',
          component: PlatformPayment, meta: { requiresAuth: true }
        },
        {
          path: 'levele-distribute',
          name: 'LeveleDistribute',
          component: LeveleDistribute, meta: { requiresAuth: true }
        },        
        {
          path: 'keep-player',
          name: 'KeepPlayer',
          component: KeepPlayer, meta: { requiresAuth: true }
        },   
        {
          path: 'gold-rank',
          name: 'GoldRank',
          component: GoldRank, meta: { requiresAuth: true }
        }, 
        {
          path: 'online-state',
          name: 'OnlineState',
          component: OnlineState, meta: { requiresAuth: true }
        }, 
        {
          path: 'gold-table',
          name: 'GoldTable',
          component: GoldTable, meta: { requiresAuth: true }
        }                       
      ]
    },
    {
      path: '/login',
      name: 'Login',
      components: {
        blank: Login
      }
    }
  ]
})
