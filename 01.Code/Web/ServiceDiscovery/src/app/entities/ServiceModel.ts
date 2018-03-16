import { Component, Injectable } from '@angular/core';
import { TypeModel } from './TypeModel';
@Component({
    template: ''
})
export class ServiceModel {
    name: string;
    fullName: string;
    description: string;
    type: string;
    route: string;
    returnArg: TypeModel;
    args: Array<TypeModel>;
    id: string;
    category: string;
    friendlyID: string;
    httpMethod: string;
}