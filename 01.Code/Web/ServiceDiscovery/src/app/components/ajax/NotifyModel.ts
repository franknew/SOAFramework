import { Component } from '@angular/core';


@Component({
    template: ''
})
export class NotifyModel {
    callback: Function;
    args: Array<any>;

    Notify(params: Array<any>) {
        let args: Array<any> = new Array<any>();
        if (params != null && params.length > 0) {
            for (let p of params) {
                args.push(p);
            }
        }
        if (this.args != null && this.args.length > 0) {
            for (let a of this.args) {
                args.push(a);
            }
        }
        if (this.callback != null) Reflect.apply(this.callback, this, args);
    }
}