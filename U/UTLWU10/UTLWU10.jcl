#UTLWU10.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWU10.LWU10R1
then
rm ${reportdir}/ULWU10.LWU10R1
fi
if test -a ${reportdir}/ULWU10.LWU10R2
then
rm ${reportdir}/ULWU10.LWU10R2
fi
if test -a ${reportdir}/ULWU10.LWU10RZ
then
rm ${reportdir}/ULWU10.LWU10RZ
fi

# run the program

sas ${codedir}/UTLWU10.sas -log ${reportdir}/ULWU10.LWU10R1  -mautosource
