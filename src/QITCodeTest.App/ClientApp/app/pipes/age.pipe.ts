import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'age' })
export class AgePipe implements PipeTransform {
    transform(value: string): any {
        if (!value) {
            return value;
        }
        const dateValue = Date.parse(value);
        if (dateValue === NaN) {
            return value;
        }

        const ageDiff = Date.now() - dateValue;
        const ageDate = new Date(ageDiff);
        return Math.abs(ageDate.getUTCFullYear() - 1970);
    }
}
