import { Pipe, PipeTransform } from '@angular/core';
import { INote } from './model/Note';

@Pipe({
  name: 'noteName'
})
export class NoteNamePipe implements PipeTransform {

  private names = [
    'C',
    'C♯',
    'D',
    'E♭',
    'E',
    'F',
    'F♯',
    'G',
    'G♯',
    'A',
    'B♭',
    'B',
  ];

  transform(value: INote, ...args: any[]): any {
    return [this.names[value.Number % 12], value.Octave].join('');
  }

}
