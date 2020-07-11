import { Pipe, PipeTransform } from '@angular/core';
import { Instruments, IInstrument } from './model/GeneralMidiInstrumentList';

@Pipe({
  name: 'instrumentName'
})
export class InstrumentNamePipe implements PipeTransform {

  private instruments = Instruments;

  transform(value: number, property: 'family'|'instrument' = 'instrument'): string {
    return this.instruments[value][property];
  }

}
