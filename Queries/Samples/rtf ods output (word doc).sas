ods listing close;					*this stops the output from going to the Output window;
ods rtf file='odsrtf_output.rtf';	*this turns on RTF output to the specified file in the /V8 directory;

proc print; *or proc freq/report... any proc with output;
run;

ods rtf close;			*stops RTF output to file;
ods listing;			*turns regular output to the Output window back on;