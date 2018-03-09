import { Component, Injectable } from '@angular/core';
import { TypeModel } from './TypeModel';
@Component({
    template: ''
})
export class ServiceModel {
    Name: string;
    Return: TypeModel;
    Args: TypeModel[];
    Description: string;
    Type: string;
    Category: string;
    Route: string;
    ID: string; 
    HttpMethod: string;
    FriendlyID: string;
}