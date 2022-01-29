#UTLWQ27.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWQ27.LWQ27R1
then
rm ${reportdir}/ULWQ27.LWQ27R1
fi
if test -a ${reportdir}/ULWQ27.LWQ27R2
then
rm ${reportdir}/ULWQ27.LWQ27R2
fi
if test -a ${reportdir}/ULWQ27.LWQ27RZ
then
rm ${reportdir}/ULWQ27.LWQ27RZ
fi

# run the program

sas ${codedir}/UTLWQ27.sas -log ${reportdir}/ULWQ27.LWQ27R1  -mautosource
