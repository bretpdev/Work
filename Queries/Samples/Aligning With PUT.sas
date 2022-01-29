/*Aligning with a Put Function*/
data temp;
 
alphabetw='  a  ';
chardate=put(alphabetw,$10. -l);
output ;
chardate=put(alphabetw,$10. -c);
output ;
chardate=put(alphabetw,$10. -r);
output ;
 
run;