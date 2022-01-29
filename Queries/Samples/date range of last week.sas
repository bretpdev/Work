data _null_  ;
   begin=intnx('week',today()+3,-1,'beginning')  ;
   call symput  ( 'begin', "'"||put(begin,yymmdd10.)||"'" )   ;

   end=intnx('week',today()+3,-1,'end')  ;
   call symput  ( 'end', "'"||put(end,yymmdd10.)||"'" )   ;
run;