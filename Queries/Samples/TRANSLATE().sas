%macro  zdmfc (TVAR, L, NTVAR)  ;
      IF  'p' <=  substr(&TVAR,&L,1)  <= 'y'
  THEN &NTVAR = TRANSLATE (&TVAR, '0123456789', 'pqrstuvwxy') * -.01;
   ELSE     &NTVAR  =  &TVAR * .01;
  %mend zdmfc;

  %zdmfc(MR1_3PIa,9,MR1_3PI)