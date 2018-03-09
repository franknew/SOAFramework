
import { Component, Injectable } from '@angular/core';
import { AjaxComponent } from '../components/ajax/ajax.component';
import { HttpClient } from '@angular/common/http';
import { ServiceModel } from '../entities/ServiceModel';
import { NotifyModel } from '../components/ajax/NotifyModel';
import { NzNotificationService } from 'ng-zorro-antd';
@Component({
    template: ''
})
@Injectable()
export class DiscoveryService {
    ajax: AjaxComponent;

    constructor (
        private http: HttpClient,
        private notify: NzNotificationService, 
    ) {
        this.ajax = new AjaxComponent(this.http);
    }

    getServiceList(success: NotifyModel[], failed: NotifyModel[] = []) {
        let url = "ServiceDiscovery/GetServiceList";
        failed.push(this.getFailedNotify());
        this.ajax.DoGet(url, null, success, failed);
    }

    getServiceInfo(id: string, success: NotifyModel[], failed: NotifyModel[] = []) {
        let url = "ServiceDiscovery/GetService/" + id;
        failed.push(this.getFailedNotify());
        this.ajax.DoGet(url, null, success, failed);
    }

    getTypeInfo(typeName: string, success: NotifyModel[], failed: NotifyModel[] = []) {
        while (typeName.indexOf(".") > -1) {
            typeName = typeName.replace(".", "-");
        }
        let url = "ServiceDiscovery/GetType/" + typeName;
        failed.push(this.getFailedNotify());
        this.ajax.DoGet(url, null, success, failed);
    }

    test(param: object, service: ServiceModel, success: NotifyModel[], failed: NotifyModel[] = []) {
        let url = service.Route;
        failed.push(this.getFailedNotify());
        if (service.HttpMethod == "GET" || service.HttpMethod == "DELETE") {
            var properties = Object.getOwnPropertyNames(param);
            for (let property of properties) {
                let v = "{" + property + "}";
                url = url.replace(v, param[property]);
            }
            this.ajax.DoGet(url, null, success, failed);
        }
        else {
            this.ajax.DoPost(url, param, success, failed);
        }
    }

    getFailedNotify(): NotifyModel {
        let loginFailed = new NotifyModel();
        loginFailed.args = [this];
        loginFailed.callback = (error, sender)=> {
                this.notify.error("发生错误", JSON.stringify(error.error));
        }
        return loginFailed;
    }
}