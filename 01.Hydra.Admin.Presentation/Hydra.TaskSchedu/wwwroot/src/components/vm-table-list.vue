<template>
  <Row class="vm-table vm-panel">
    <div class="panel-heading">
      {{ title }}
    </div>
    <div class="panel-body">
      <Row type="flex" justify="space-between" class="control">
        <div class="search-bar">
          <Input placeholder="Please enter ..." v-model="keyword" style="width: 300px"></Input>
          <Button type="ghost" @click="search"><i class="fa fa-search"></i></Button>
        </div>
      </Row>
      <div class="edit" v-if="type === 'edit'">
          <Button @click="modalAdd = true" ><i class="fa fa-plus"></i> 新增</Button>
          <Button  :disabled="deleteDisabled" @click="modalDelete = true"><i class="fa fa-trash"></i> 删除</Button>
      </div>
      <Table :stripe="showStripe" :size="tableSize" :columns="showColumns" :data="dataShow" @on-selection-change="selectChange"></Table>
      <Row type="flex" justify="space-between" class="footer">
        <div class="page">
          <Page :total="total" :current="currentPage" :page-size="showNum" @on-change="pageChange" show-total show-elevator></Page>
        </div>
      </Row>
    </div>
    <Modal
        v-model="modalDelete"
        title="删除"
        ok-text="确定"
        cancel-text="取消"
        v-on:on-ok="deleteOk">
        确定删除选中数据?
    </Modal>
  </Row>
</template>

<script>
  export default {
    name: 'VmTable',
    props: {
      title: {
        type: String,
        default: 'Basic Table'
      },
      type: String,
      columns: Array,
      data: Array,
      total:0
    },
    data () {
      return {
        deleteDisabled: true,
        dataShow: [],
        showNum: 50,
        showStripe: true,
        tableSize: 'small',
        currentPage: 1,
        keyword: '',
        modalEdit: false,
        modalAdd: false,
        modalDelete: false,
        dataEdit: {},
        dataDelete: [],
        dataAdd: {},
        SerializeForm: []
      }
    },
    methods: {
      deleteOk: function () {
        this.$emit('delete-ok', this.dataDelete)
      },
      pageChange: function (page) {
        this.currentPage = page
        this.updateDataShow()
      },
      updateDataShow: function () {
        let startPage = (this.currentPage - 1) * this.showNum
        let endPage = startPage + this.showNum
        this.dataShow = this.data.slice(startPage, endPage)
      },
      search: function () {
        let that = this
        let tempData = that.data
        that.dataShow = []
        tempData.forEach(function (elem) {
          for (let i in elem) {
            if (elem[i].toString().indexOf(that.keyword) > -1) {
              that.dataShow.push(elem)
              return
            }
          }
        })
      },
      selectChange: function (data) {
        this.dataDelete = data
      },
      remove: function (index) {
        this.dataShow.splice(index, 1)
      },
      renderOperate: function (h, params) {
        return h('div', [
          h('Button', {
            props: {
              type: 'info',
              size: 'small'
            },
            style: {
              marginRight: '5px'
            },
            on: {
              click: () => {
                for (let i in params.row) {
                  if (i !== '_index' && i !== '_rowKey') {
                    this.dataEdit[i] = params.row[i]
                  }
                }
                this.modalEdit = true
              }
            }
          }, '编辑'),
          h('Button', {
            props: {
              type: 'error',
              size: 'small'
            },
            on: {
              click: () => {
                this.dataDelete.push(params.row)
                this.modalDelete = true
              }
            }
          }, '删除')
        ])
      },
      convertKey: function (value) {
        let returnValue = value
        this.columns.forEach(function (elem) {
          for (let i in elem) {
            if (i === 'key' && elem[i] === value) {
              returnValue = elem.title
            }
          }
        })
        return returnValue
      }
    },
    computed: {
      showColumns: function () {
        let showColumn = this.columns.slice()
        showColumn.forEach(function (elem) {
          elem.sortable = true
        })
        if (this.type === 'edit') {
          showColumn.unshift({
            type: 'selection',
            width: 60,
            align: 'center'
          })
          showColumn.push({
            title: '操作',
            key: 'action',
            width: 150,
            align: 'center',
            render: this.renderOperate
          })
        }
        return showColumn
      }
    },
    watch: {
      data: function () {
        this.dataShow = this.data.slice(0, this.showNum)
      },
      dataDelete: function () {
        if (this.dataDelete.length === 0) {
          this.deleteDisabled = true
        } else {
          this.deleteDisabled = false
        }
      }
    },
    mounted: function () {
      this.dataShow = this.data.slice(0, this.showNum)
    }
  }
</script>
