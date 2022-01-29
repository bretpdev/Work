#UTLWR18.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWR18.LWR18R1
then
rm ${reportdir}/ULWR18.LWR18R1
fi
if test -a ${reportdir}/ULWR18.LWR18R2
then
rm ${reportdir}/ULWR18.LWR18R2
fi

# run the program

sas ${codedir}/UTLWR18.sas -log ${reportdir}/ULWR18.LWR18R1  -mautosource
