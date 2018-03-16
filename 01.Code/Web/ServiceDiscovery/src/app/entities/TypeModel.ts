import { Component, Injectable } from '@angular/core';
@Component({
    template: ''
})
export class TypeModel {
    memberName: string;
    name: string;
    fullName: string;
    description: string;
    nameSpace: string;
    genericArguments: Array<TypeModel>;
    properties: Array<TypeModel>;
    index: number;
    isClass: boolean;
    isArray: boolean;
    id: string;
}