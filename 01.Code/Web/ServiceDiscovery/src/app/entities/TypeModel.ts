import { Component, Injectable } from '@angular/core';
@Component({
    template: ''
})
export class TypeModel {
    ID: string;
    MemberName: string;
    Name: string;
    FullName: string;
    Description: string;
    NameSpace: string;
    GenericArguments: TypeModel[];
    Properties: TypeModel[];
    IsClass: boolean;
    IsArray: boolean;
}