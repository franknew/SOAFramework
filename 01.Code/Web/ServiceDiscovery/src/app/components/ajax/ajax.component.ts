import { Component, ErrorHandler } from '@angular/core';
import 'reflect-metadata';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { NotifyModel } from './NotifyModel';

declare var $ :any;

@Component({
    moduleId: "ajax",
    selector: 'ajax',
    template: '',
})
export class AjaxComponent {
    lock: boolean = false;

    constructor(private http: HttpClient) {}

    buildQueryString(url:string, data: any) : string {
        let fullUrl = url;
        if (data != null) {
            var properties = Object.getOwnPropertyNames(data);
            for (var i = 0; i < properties.length; i++) {
                if (fullUrl.indexOf("?") < 0) {
                    fullUrl += "?";
                }
                fullUrl += properties[i] + "=" + data[properties[i]] + "&";
            }
        }
        return fullUrl;
    }

    Get<T>(url: string, data: any = null, failed: Function = null) : Promise<T>
    {
        try
        {
            let fullUrl = this.buildQueryString(url, data);
            let response: T;
            this.http.get<T>(fullUrl).subscribe(data=>{
                return response = data;
            });
            // await $.get(fullUrl, function(datat) {
            //     response = data;
            // });
            return Promise.resolve(response);
        }
        catch (ex) 
        {
            if (failed != null) failed(ex);
        }
    }

    DoGet(url: string, data: any = null, successNotify: Array<NotifyModel>, failedNotify: Array<NotifyModel> = null) {
        let fullUrl = this.buildQueryString(url, data);
        let response: any;
        this.http.get(fullUrl).subscribe(data=>{
            if (successNotify != null && successNotify.length > 0) {
                for (let n of successNotify) {
                    n.Notify([data]);
                }
            }
        }, (err: HttpErrorResponse)=>{
            if (failedNotify != null && failedNotify.length > 0) {
                for (let n of failedNotify) {
                    n.Notify([err]);
                }
            }
        });
    }
    
    async Post<T>(url: string, data: any, failed: Function = null) 
    {
        try
        {
            let response: any;
            await $.post(url, data, function(result: any) {
                return response = result;
            }, 'json');
            return <T>response;
        }
        catch (ex)
        {
            if (failed != null) failed(ex);
        }
    }

    DoPost(url: string, data: any, successNotify: Array<NotifyModel>, failedNotify: Array<NotifyModel> = null) {
        this.http.post(url, data).subscribe(data=>{
            if (successNotify != null && successNotify.length > 0) {
                for (let n of successNotify) {
                    n.Notify([data]);
                }
            }
        }, (err: HttpErrorResponse)=>{
            if (failedNotify != null && failedNotify.length > 0) {
                for (let n of failedNotify) {
                    n.Notify([err]);
                }
            }
        });
    }
}
