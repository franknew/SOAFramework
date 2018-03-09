import { Component, Input, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { ServiceModel } from '../../entities/ServiceModel';
import { NzModalService, NzModalSubject } from 'ng-zorro-antd';
import { NotifyModel } from '../ajax/NotifyModel';
import { DiscoveryService } from '../../services/DiscoveryService';
import { TypeModel } from '../../entities/TypeModel';

declare var $ :any;
@Component({
    moduleId: 'service-list',
    selector: 'service-list',
    templateUrl: 'service-list.component.html',
})
export class ServiceListComponent implements OnInit {
    modal: NzModalSubject;
    selectedCategory: string;
    allServiceList: Array<ServiceModel>= [];
    displayServiceList: Array<ServiceModel> = [];
    operationTitle: string;
    selectedService: ServiceModel;
    selectedType: TypeModel;
    testLoading: boolean = false;
    typeLoading: boolean = false;
    paramObj: Object;
    testResponse: string = "还没有对接口进行测试";
    loading: boolean = false;
    titlestring: string;

    @ViewChild("title") 
    title: TemplateRef<any>;
    @ViewChild("footer")
    footer: TemplateRef<any>; 
    @ViewChild("serviceDetail")
    content: TemplateRef<any>; 
    @ViewChild("testFooter")
    testFooter: TemplateRef<any>; 
    @ViewChild("testContent")
    testContent: TemplateRef<any>; 
    @ViewChild("typeContent")
    typeContent: TemplateRef<any>; 

    constructor(
        private modalService: NzModalService,
        private service: DiscoveryService,
    ){}

    ngOnInit(): void {
    }

    @Input()
    set SelectedCategory(category: string) {
        this.selectedCategory = category;
        this.displayServiceList = new Array<ServiceModel>();
        for (let service of this.allServiceList) {
            if (service.Category == this.selectedCategory || this.selectedCategory == "All") {
                this.displayServiceList.push(service);
            }
        }
    }

    @Input()
    set ServiceList(list: ServiceModel[]) {
        if (list == null) this.allServiceList = [];
        else this.allServiceList = list;
        if (this.selectedCategory == "All") this.displayServiceList = list;
    }

    @Input()
    set Loading(loading: boolean) {
        this.loading = loading;
    }

    diaplayDetail(service: ServiceModel) {
        this.selectedService = service;
        this.titlestring = this.selectedService.Route;
        let id = service.Route.replace("/", "-").replace("{","-").replace("}","");

        this.modal = this.modalService.open({
            title       : this.title,
            content     : this.content,
            footer      : this.footer,
            maskClosable: false,
            closable    : true,
            width       : 1500,
            wrapClassName: "vertical-center-modal",
            zIndex      : 100,
          });
    }

    displayType(type: TypeModel) {
        this.changeType(type);
        this.modal = this.modalService.open({
            title       : this.title,
            content     : this.typeContent,
            footer      : this.footer,
            maskClosable: false,
            closable    : true,
            width       : 1500,
            wrapClassName: "vertical-center-modal",
            zIndex      : 101,
          });
    }

    changeType(type: TypeModel) {
        this.typeLoading = true;
        this.titlestring = type.Name;
        let success = new NotifyModel();
        success.callback = (data)=> {
            this.selectedType = data;
            this.typeLoading = false;
        }
        let failed = new NotifyModel();
        failed.callback = (err)=> {
            this.typeLoading = false;
        }
        this.service.getTypeInfo(type.FullName, [success], [failed]);
    }

    displayTest(service: ServiceModel) {
        this.titlestring = this.selectedService.Route;
        this.testResponse = "还没有对接口进行测试";
        this.selectedService = service;
        this.paramObj = new Object();        
        for (let property of this.selectedService.Args) {
            this.paramObj[property.MemberName] = null;
        }

        this.modal = this.modalService.open({
            title       : this.title,
            content     : this.testContent,
            footer      : this.testFooter,
            maskClosable: false,
            closable    : true,
            width       : 1500,
            wrapClassName: "vertical-center-modal",
            zIndex      : 100,
          });
    }

    test() {
        this.testLoading = true;
        var properties = Object.getOwnPropertyNames(this.paramObj);
        for (let property of properties) {
            this.paramObj[property] = $("#" + property + " input").val();
        }
        let success = new NotifyModel();
        success.callback = (data)=> {
            this.testResponse = JSON.stringify(data);
            this.testLoading = false;
        }
        let failed = new NotifyModel();
        failed.callback = (err)=> {
            this.testResponse = err.error;
            this.testLoading = false;
        }
        this.service.test(this.paramObj, this.selectedService, [success], [failed]);
    }
}
