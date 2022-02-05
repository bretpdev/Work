#UNWR08.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWR08.NWR08R1
then
rm ${reportdir}/UNWR08.NWR08R1
fi
if test -a ${reportdir}/UNWR08.NWR08RZ
then
rm ${reportdir}/UNWR08.NWR08RZ
fi
if test -a ${reportdir}/UNWR08.NWR08R2
then
rm ${reportdir}/UNWR08.NWR08R2
fi


# run the program

sas ${codedir}/UTNWR08.sas -log ${reportdir}/UNWR08.NWR08R1  -mautosource
